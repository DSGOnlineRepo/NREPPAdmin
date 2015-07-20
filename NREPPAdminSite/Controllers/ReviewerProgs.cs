using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using NREPPAdminSite.Models;
using System.Data.Entity;

namespace NREPPAdminSite.Controllers
{
    [Authorize]
    public partial class ProgramController : Controller
    { 
        public ActionResult ScreenResults(int InvId)
        {
            List<Study> theStudies = new List<Study>();
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            Intervention theIntervention;
            List<Answer> StudyDesigns;
            List<Answer> YPYN;
            List<Answer> Exclusions;
            List<RCDocument> reviewerDocs;



            theStudies = localService.GetStudiesByIntervention(InvId).ToList<Study>();
            StudyDesigns = localService.GetAnswersByCategory("StudyDesign").ToList<Answer>();
            YPYN = localService.GetAnswersByCategory("YPYN").ToList<Answer>();
            Exclusions = localService.GetAnswersByCategory("Exclusions").ToList<Answer>();

            //List<Object> something = theStudies.GroupBy(s => s.StudyId).Select(group => new { StudyId = group.Key });

            //theIntervention = localService.GetInterventions(InterventionId);

            SqlParameter idParam = new SqlParameter() { ParameterName = "@Id", SqlDbType = SqlDbType.Int, Value = InvId };
            SqlParameter roleParam = new SqlParameter() { ParameterName = "@UserName", Value = User.Identity.Name };
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(idParam);
            parameters.Add(roleParam);
            var interventionList = localService.GetInterventions(parameters);
            theIntervention = interventionList.Interventions.FirstOrDefault();

            OutcomesWrapper ow = localService.GetOutcomesByIntervention(InvId);

            List<OutcomeMeasure> oms = ow.OutcomesMeasures.Where(om => om.OutcomeId == 1).ToList<OutcomeMeasure>();
            reviewerDocs = localService.GetRCDocuments(null, theIntervention.Id);

            ScreeningModel sm = new ScreeningModel(theStudies, theIntervention, StudyDesigns, YPYN, Exclusions, ow); // TODO: fix this call
            sm.AddDests(localService.GetDestinations(theIntervention.Id).ToList());

            sm.TheDocuments = reviewerDocs;

            return View(sm);
        }

        public ActionResult SaveNotes(ScreeningModel sm, FormCollection col) // Alright, but it should work with something
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            localService.SaveIntervention(sm.TheIntervention);

            string destination = col["Redirect"];


            // TODO: Fix this redirect
            return RedirectToAction("Screen", new { InvId = sm.TheIntervention.Id });
        }

        public ActionResult ReviewRigor(int InvId)
        {
            NrepServ localService = NrepServ.GetLocalService();

            // Get all the information to build the rest of the page

            List<RCDocument> AllDocs = localService.GetRCDocuments(null, InvId);

            OutcomesWrapper ow = localService.GetOutcomesByIntervention(InvId);
            List<Study> theStudies = localService.GetStudiesByIntervention(InvId).ToList<Study>();

            List<int> docIds = theStudies.Where(s => s.RecommendReview == true).Select(s => s.DocumentId).ToList();

            //docIds.Concat(theStudies.Where(s => s.RecommendReview == true).Select(s => s.DocumentId));
            docIds = docIds.Concat(ow.OutcomesMeasures.Where(o => o.RecommendReview == true).Select(o => o.DocumentId)).ToList();


            List<RCDocument> ReviewDocs = AllDocs.Where(d => docIds.Contains(d.DocId)).Distinct().ToList();
            List<RCDocument> Supplementals = AllDocs.Where(d => d.AddToReview == true && d.DocumentTypeName != "Program Evaluation").ToList();

            ReviewerDocsWrapper theDocs = new ReviewerDocsWrapper(ReviewDocs, Supplementals);

            /*foreach (Study s in theStudies)
            {
                if (s.RecommendReview)
                    docIds.Add(s.DocumentId);
            }

            foreach(OutcomeMeasure om in ow.OutcomesMeasures)
            {
                if (om.RecommendReview)
                {
                    docIds.Add()
                }
            }*/

            

            return View(theDocs);
        }

        public ActionResult Consensus()
        {
            List<QoRAnswer> answers = new List<QoRAnswer>();
            //return View(answers);
            return View();
        }

        public ActionResult SAMHSAApprove(int InvId)
        {
            NrepServ localService = NrepServ.GetLocalService();

            Intervention inv = localService.GetIntervention(InvId, User.Identity.Name);

            @ViewBag.InvId = InvId;
            @ViewBag.ProgramName = inv.Title;
            @ViewBag.IsLive = inv.IsLive;
            return View();
        }

        [HttpGet]
        public ActionResult PostToSite(bool SiteVal, int InvId)
        {
            NrepServ localService = NrepServ.GetLocalService();

            localService.SetSitePosting(SiteVal, InvId);

            if (SiteVal)
                return Json("You posted!", JsonRequestBehavior.AllowGet);
            else return Json("You delisted", JsonRequestBehavior.AllowGet);
        }
    }
}