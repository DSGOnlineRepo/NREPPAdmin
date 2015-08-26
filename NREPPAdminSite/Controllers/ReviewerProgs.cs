using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using NREPPAdminSite.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NREPPAdminSite.Context;

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

            return View(theDocs);
        }

        public ActionResult Consensus(int InvId)
        {

           
            MyIdentityDbContext db = new MyIdentityDbContext();
            UserStore<ExtendedUser> userStore = new UserStore<ExtendedUser>(db);
            UserManager<ExtendedUser> _userManager = new UserManager<ExtendedUser>(userStore);

            ConsensusModel model;
            NrepServ localService = NrepServ.GetLocalService();

            List<QoRAnswerType> questions = localService.GetQoRAnswerTypes().ToList();
            List<QoRAnswer> FinalAnswers = localService.GetFinalAnswers(InvId).ToList();
            OutcomesWrapper ow = localService.GetOutcomesByIntervention(InvId);

            List<string> ReviewerIds = FinalAnswers.Select(x => x.ReviewerId).Distinct().ToList(); // Should only be two but this ensures that
            Dictionary<string, string> ReviewerNames = new Dictionary<string, string>();

            // Since there are only two reviewers no need for a foreach
            ReviewerNames.Add(ReviewerIds[0], _userManager.FindById(ReviewerIds[0]).FirstName);
            ReviewerNames.Add(ReviewerIds[1], _userManager.FindById(ReviewerIds[1]).FirstName);

            foreach (QoRAnswer ans in FinalAnswers)
                ans.ReviewerName = ReviewerNames[ans.ReviewerId];
            

            model = new ConsensusModel(FinalAnswers, questions, ow);
            model.AssessmentPd = localService.GetAnswersByCategory("AssessmentPd");
            model.SampleSize = localService.GetAnswersByCategory("FullSample");

            return View(model);
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

        [HttpGet]
        public ActionResult AcceptInvite(string Token)
        {
            string[] tokenResults = NrepServ.DecryptInvite(Token);
            NrepServ localService = NrepServ.GetLocalService();

            // TODO: Collision Accept Twice

            bool didWork = localService.AcceptOrDeclineReview(int.Parse(tokenResults[0]), tokenResults[1], true);

            // TODO: Emails

            /*if (didWork)
                return Json("Accepted!", JsonRequestBehavior.AllowGet);
            else return Json("Error!", JsonRequestBehavior.AllowGet);*/

            return RedirectToAction("ReviewerResponse", new {Accept = true, InToken = Token});
        }

        [HttpGet]
        public ActionResult DeclineInvite(string Token)
        {
            string[] tokenResults = NrepServ.DecryptInvite(Token);
            NrepServ localService = NrepServ.GetLocalService();

            // TODO: Collision (decline twice)


            bool didWork = localService.AcceptOrDeclineReview(int.Parse(tokenResults[0]), tokenResults[1], false);

            // TODO: Emails

            /*if (didWork)
                return Json("Accepted!", JsonRequestBehavior.AllowGet);
            else return Json("Error!", JsonRequestBehavior.AllowGet);*/

            return RedirectToAction("ReviewerResponse", new {Accept = false, InToken = Token});


        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult RTFPost(FormCollection col)
        {
            RTFTemp temp = new RTFTemp();
            temp.Text1 = col["html_content"];

            return View("RTFTest", temp);
        }

        public ActionResult ReviewerResponse(bool Accept, string InToken)
        {
            List<Answer> TempAnswers = new List<Answer>(); // Replace this with the answers once you have them in DB
            string[] tokenResults = NrepServ.DecryptInvite(InToken);
            NrepServ localService = NrepServ.GetLocalService();
            List<Answer> responseAnswers = localService.GetAnswersByCategory("ReviewerResponse").ToList();

            TempAnswers.Add(new Answer(1, "Answer Number 1", "One"));
            TempAnswers.Add(new Answer(2, "Answer Number 2", "Two"));
            TempAnswers.Add(new Answer(3, "Answer Write In", "Other"));

            ReviewerAcceptancePageModel model = new ReviewerAcceptancePageModel(Accept, responseAnswers);
            model.InvId = int.Parse(tokenResults[0]);
            model.UserId = tokenResults[1];
            ViewBag.ProgramName = "[Program Name Goes Here]";

            return View(model);
        }
        
        [HttpPost]
        public ActionResult SaveResponse(FormCollection col)
        {
            NrepServ localService = NrepServ.GetLocalService();

            string userId = col["UserId"];
            int InvId = int.Parse(col["InvId"]);
            int responseVal = int.Parse(col["multiAnswer"]);

            List<ReviewerOnInterv> assignedReviewers = localService.GetReviewersByIntervention(InvId);
            RegisterViewModel rvm = localService.GetReviewerByUserId(userId);

            string currentStatus = assignedReviewers.Where(x => x.Id == rvm.Id).Select(m => m.ReviewerStatus).First();

            localService.AssignReviewer(InvId, userId, null, currentStatus, responseVal, col["otherAnswer"]);


            return View("PostResponse");
        }
    }


    public class RTFTemp
    {
        public string Text1 { get; set; }
    }
}