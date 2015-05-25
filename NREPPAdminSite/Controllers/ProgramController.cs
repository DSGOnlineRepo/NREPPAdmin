using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NREPPAdminSite.Constants;
using NREPPAdminSite.Models;
using NREPPAdminSite.Security;
using System.Globalization;

namespace NREPPAdminSite.Controllers
{
    [Authorize]
    public partial class ProgramController : Controller
    {

         private readonly UserManager<ExtendedUser> _userManager;

        public ProgramController()
        {
            MyIdentityDbContext db = new MyIdentityDbContext();

            UserStore<ExtendedUser> userStore = new UserStore<ExtendedUser>(db);
            _userManager = new UserManager<ExtendedUser>(userStore);
         }

        #region Get Methods

        // GET: Program
        public ActionResult View(int InvId)
        {
            ViewBag.Title = "Program Title";
            ViewBag.Id = InvId;
            IntervPageModel pageModel;
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            
            // Probably don't need to seed these

            //List<Answer> docTypez = new List<Answer>(); 
            List<MaskValue> intervTypez = new List<MaskValue>();

            Intervention theIntervention;

            List<Answer> documentTypes = localService.GetAnswersByCategory("DocumentType").ToList<Answer>();
            List<MaskValue> programTypes = localService.GetMaskList("ProgramType").ToList<MaskValue>();
            List<MaskValue> preScreen = localService.GetMaskList("PreScreen").ToList<MaskValue>();
            List<InterventionDoc> documentz;
            List<RCDocument> reviewerDocs;

            SubmissionPd pd = localService.GetCurrentSubmissionPd();
            ViewBag.StartDate = pd.StartDate.ToString("MMM", CultureInfo.InvariantCulture) + ", " + pd.StartDate.Year.ToString();
            ViewBag.EndDate = pd.EndDate.ToString("MMM", CultureInfo.InvariantCulture) + ", " + pd.EndDate.Year.ToString();

            if (InvId > 0)
            {
                var user = _userManager.FindByName(User.Identity.Name);
                var userRoles = _userManager.GetRoles(user.Id);
                SqlParameter idParam = new SqlParameter() { ParameterName = "@Id", SqlDbType = SqlDbType.Int, Value = InvId };
                SqlParameter roleParam = new SqlParameter() { ParameterName = "@UserName", Value = User.Identity.Name };
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(idParam);
                parameters.Add(roleParam);
                List<Intervention> interventionList = localService.GetInterventions(parameters);
                theIntervention = interventionList[0];

                documentz = localService.GetDocuments(InvId, null, null).ToList<InterventionDoc>();
                reviewerDocs = localService.GetRCDocuments(null, theIntervention.Id);
            }
            else
            {
                theIntervention = new Intervention(-1, "", "", User.Identity.Name, null, DateTime.Now, User.Identity.GetUserId(), "", -1, 0, "", false);
                pageModel = new IntervPageModel();
                documentz = new List<InterventionDoc>();
                reviewerDocs = new List<RCDocument>();
            }

            List<Destination> nDests = localService.GetDestinations(theIntervention.Id).ToList();

            pageModel = new IntervPageModel(documentz, reviewerDocs, MaskValue.SplitMask(programTypes, theIntervention.ProgramType).ToList<MaskValue>(),
                documentTypes, nDests, MaskValue.SplitMask(preScreen, theIntervention.PreScreenMask).ToList<MaskValue>(),
                MaskValue.SplitMask(preScreen, theIntervention.UserPreScreenMask).ToList<MaskValue>());

            List<string> perms = new List<string>();

            perms.Add("UploadDocs");
            perms.Add("SeeRCDocs");
            perms.Add("EditBaseSubmission");
            perms.Add("UserPreScreen");

            pageModel.SetPermissions(perms, User.Identity.Name, InvId);
            pageModel.TheIntervention = theIntervention;

            return View(pageModel);
        }


        // TODO: Make sure you have a cookie and rights to delete this document (pretty easy, but you DO need to implement it)

        public ActionResult DeleteDocument(int DocId, int InvId)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);

            localService.DeleteDocument(DocId, 1); // TODO: Get User from Cookie

            return RedirectToAction("View", new { InvId = InvId });
        }

        /// <summary>
        /// Deletes an Outcome Measure Record
        /// </summary>
        /// <param name="RecId">The Record Id to Delete</param>
        /// <param name="InvId">The Program ID so it can redirect</param>
        /// <returns></returns>
        public ActionResult DeleteOutcomeMeasure(int MeasureId, int InvId)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            
            localService.DeleteOutcomeMeasure(MeasureId);

            return RedirectToAction("Screen", new { InterventionId = InvId });
        }

        /// <summary>
        /// Deletes a Study Record
        /// </summary>
        /// <param name="RecId">The Record Id to Delete</param>
        /// <param name="InvId">The Program ID so it can redirect</param>
        /// <returns></returns>
        public ActionResult DeleteStudyRecord(int RecId, int InvId)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);

            localService.DeleteStudyRecord(RecId);

            return RedirectToAction("Screen", new { InterventionId = InvId });
        }

        public ActionResult UploadFile()
        {
            return View();
        }

        public string Document()
        {

            return "Something came out!";
        }

        public ActionResult Screen(int InterventionId)
        {
            List<Study> theStudies = new List<Study>();
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            Intervention theIntervention;
            List<Answer> StudyDesigns;
            List<Answer> YPYN;
            List<Answer> Exclusions;
            List<RCDocument> reviewerDocs;
            List<Answer> AttritionAnswers;
            List<Answer> SAMHSAOut;
            List<Answer> SAMHSAPop;
            List<Answer> EffectReports;
            List<Answer> TaxOutcomes;
            
            
            theStudies = localService.GetStudiesByIntervention(InterventionId).ToList<Study>();
            StudyDesigns = localService.GetAnswersByCategory("StudyDesign").ToList<Answer>();
            YPYN = localService.GetAnswersByCategory("YPYN").ToList<Answer>();
            Exclusions = localService.GetAnswersByCategory("Exclusions").ToList<Answer>();
            AttritionAnswers = localService.GetAnswersByCategory("AttritionAnswer").ToList<Answer>();
            SAMHSAOut = localService.GetAnswersByCategory("SAMHSAOutcome").ToList<Answer>();
            SAMHSAPop = localService.GetAnswersByCategory("SAMHSAPop").ToList<Answer>();
            EffectReports = localService.GetAnswersByCategory("TreatCompare").ToList<Answer>();
            TaxOutcomes = localService.GetTaxonomicOutcomes(null).ToList<Answer>();

            //List<Object> something = theStudies.GroupBy(s => s.StudyId).Select(group => new { StudyId = group.Key });

            //theIntervention = localService.GetInterventions(InterventionId);

            SqlParameter idParam = new SqlParameter() { ParameterName = "@Id", SqlDbType = SqlDbType.Int, Value = InterventionId };
            SqlParameter roleParam = new SqlParameter() { ParameterName = "@UserName", Value = User.Identity.Name };
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(idParam);
            parameters.Add(roleParam);
            List<Intervention> interventionList = localService.GetInterventions(parameters);
            theIntervention = interventionList[0];

            OutcomesWrapper ow = localService.GetOutcomesByIntervention(InterventionId);

            //List<OutcomeMeasure> oms = ow.OutcomesMeasures.Where(om => om.OutcomeId == 1).ToList<OutcomeMeasure>();
            List<OutcomeMeasure> oms = ow.OutcomesMeasures.ToList<OutcomeMeasure>();
            reviewerDocs = localService.GetRCDocuments(null, theIntervention.Id);

            ScreeningModel sm = new ScreeningModel(theStudies, theIntervention, StudyDesigns, YPYN, Exclusions, ow, reviewerDocs, AttritionAnswers,
                SAMHSAPop, SAMHSAOut, EffectReports, TaxOutcomes);
            sm.AddDests(localService.GetDestinations(theIntervention.Id).ToList());

            return View(sm);
        }

        #endregion

        #region Post Methods

        /// <summary>
        /// Performs the submission of a new program
        /// </summary>
        /// <param name="model">The intervention to submit for review</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(IntervPageModel model, FormCollection col)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);

            int destLoc = 0;
            string destUser = null;

            if (col["selDest"] != null)
            {

                string Destination = col["selDest"];

                string[] DestStuff = Destination.Split(',');
               destUser = DestStuff[0];
                destLoc = int.Parse(DestStuff[1]);
            }
            else
                destLoc = int.Parse(col["destStatus"]);

            int returnValue = localService.SaveIntervention(model.TheIntervention);

            localService.ChangeStatus(model.TheIntervention.Id, destUser, destLoc);

            return RedirectToAction("Programs", "Home");
        }

        [HttpPost]
        public ActionResult Edit(IntervPageModel inInterv)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            
            //inInterv.TheIntervention.SubmitterId = User.Identity.GetUserId();
            int returnValue = localService.SaveIntervention(inInterv.TheIntervention);

            return RedirectToAction("View", new { InvId = returnValue });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Upload(FormCollection formCollection)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);
           
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                    localService.SaveFileToDB(fileBytes, fileName, User.Identity.Name, "NOT IMPLEMENTED!", int.Parse(Request.Form["TheIntervention.Id"]), false,
                        -1, Request.Form["FileDescription"], int.Parse(Request.Form["docTypeDD"]), Request.Form["FileTitle"]); // TODO: Add UserId to the Cookie. :|
                }
            }

            if (formCollection["OnPage"] == "Sub4")
                return RedirectToAction("Submission4", new { InvId = int.Parse(Request.Form["TheIntervention.Id"]) });
            else 
                return RedirectToAction("View", new { InvId = int.Parse(Request.Form["TheIntervention.Id"]) });
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadFull(FormCollection col)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);

            // TODO: Put this in a function so that I can use it in the function above, too
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["RCUploadFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                    localService.SaveFileToDB(fileBytes, fileName, User.Identity.Name, "NOT IMPLEMENTED!", int.Parse(Request.Form["TheIntervention.Id"]), false,
                        -1, Request.Form["FileDescription"], int.Parse(Request.Form["docTypeDD"]), col["FileTitle"]); // TODO: Add UserId to the Cookie. :|
                }
            }

            // See Above

            string RCNameText = "txtRCName_", ReferenceText = "txtRef_", hiddenText = "hid_", dirtyText = "isdirty_";
            string PubYearText = "txtPubYear_";

            int i = 0;

            while (i < col.Count) // break if i gets too big
            {
                if (col[dirtyText + i.ToString()] == "true")
                {
                    // TODO: Check to see if an RCDocinfo Exists
                    localService.UpdateRCDocInfo(-1, int.Parse(col[hiddenText + i.ToString()]), col[ReferenceText + i.ToString()],
                        col[RCNameText + i.ToString()], col[PubYearText + i.ToString()] == string.Empty ? null : (int?)int.Parse(col[PubYearText + i.ToString()]));
                    break;
                }
                else i++;

            }

            return RedirectToAction("View", new { InvId = int.Parse(Request.Form["TheIntervention.Id"]) });
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddStudy()
        {
            //int InvId = 0;
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            Study nStudy = new Study();
            nStudy.StudyId = int.Parse(Request.Form["StudyId"]);
            nStudy.DocumentId = int.Parse(Request.Form["docDropDown"]);
            nStudy.StudyDesign = int.Parse(Request.Form["StudyDesign"]);
            nStudy.BaselineEquiv = Request.Form["BaselineEquiv"] == null ? "" : Request.Form["BaselineEquiv"];
            nStudy.TotalSampleSize = Request.Form["TotalSampleSize"] == null ? "" : Request.Form["TotalSampleSize"];
            nStudy.LongestFollowup = Request.Form["LongestFollowup"] == null ? "" : Request.Form["LongestFollowup"];
            nStudy.Reference = Request.Form["Reference"];
            nStudy.Id = Request.Form["ActualID"] == string.Empty ? -1 : int.Parse(Request.Form["ActualID"]);
            nStudy.Notes = Request.Form["Notes"];
            nStudy.UseMultivariate = Request.Form["useMultivariate"] == "on";
            nStudy.Exclusion1 = int.Parse(Request.Form["Exclusion1"]);
            nStudy.RecommendReview = Request.Form["recommendReview"] == "on";
            nStudy.OverallAttrition = int.Parse(Request.Form["OverallAttrition"]);
            nStudy.DiffAttrition = int.Parse(Request.Form["DiffAttrition"]);
            nStudy.DocOrdinal = Request.Form["articleIdBox"] == null || Request.Form["articleIdBox"].Trim() == string.Empty ? -1 :
                int.Parse(Request.Form["articleIdBox"]);

            int ActualId = localService.AddStudyRecord(nStudy);

            RCDocument rcDoc = new RCDocument(nStudy.DocumentId, int.Parse(Request.Form["RCDocumentId"]));
            rcDoc.RCName = Request.Form["RCDocumentName"];
            rcDoc.Reference = Request.Form["newReference"];
            rcDoc.PubYear = int.Parse(Request.Form["PubYear"]);

            localService.UpdateRCDocInfo(rcDoc.RCDocId, rcDoc.DocId, rcDoc.Reference, rcDoc.RCName, rcDoc.PubYear);

            return RedirectToAction("Screen", new { InterventionId = int.Parse(Request.Form["InterventionId"]) });
        }

        [HttpPost]
        public ActionResult AddOutcome(FormCollection col)
        {
            int IntervId = int.Parse(col["IntervId"]);
            
            OutcomeMeasure om = new OutcomeMeasure();
            om.Id = col["OutcomeMeasureId"] == string.Empty || int.Parse(col["OutcomeMeasureId"]) < 1 ? -1 : int.Parse(col["OutcomeMeasureId"]);
            om.DocumentId = int.Parse(col["docDropDown"]);
            om.SAMHSAOutcome = int.Parse(col["samOutDropDown"]);
            om.SAMHSAPop = int.Parse(col["popDropDown"]);
            om.EffectReport = int.Parse(col["treatmentDropDown"]);
            om.PopDescription = col["popDescription"];
            om.StudyId = int.Parse(col["studySelector"]);
            om.OutcomeMeasureName = col["measure"];
            om.OutcomeId = int.Parse(col["MacroOutcome"]);
            om.RecommendReview = col["reviewOutcome"].ToString() == "on";
            om.TaxOutcome = int.Parse(col["TaxOutcome"]);

            NrepServ localService = new NrepServ(NrepServ.ConnString);
            localService.AddOrUpdateOutcomeMeasure(om, IntervId, col["newOutcome"].Trim());


            return RedirectToAction("Screen", new { InterventionId = IntervId }); // TODO: Pass errors on failure

        }

        /// <summary>
        /// Update the Documents to include the RC Information
        /// </summary>
        /// <param name="col">Form Collection</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateRCDocs(FormCollection col)
        {
            string RCNameText = "txtRCName_", ReferenceText = "txtRef_", hiddenText = "hid_", dirtyText = "isdirty_";
            string PubYearText = "txtPubYear_";
            NrepServ localService = new NrepServ(NrepServ.ConnString);

            int i = 0;

            while(i < col.Count) // break if i gets too big
            {
                if (col[dirtyText + i.ToString()] == "true")
                {
                    // TODO: Check to see if an RCDocinfo Exists
                    localService.UpdateRCDocInfo(-1, int.Parse(col[hiddenText + i.ToString()]), col[ReferenceText + i.ToString()],
                        col[RCNameText + i.ToString()], col[PubYearText + i.ToString()] == string.Empty ? null : (int?)int.Parse(col[PubYearText + i.ToString()]));
                    break;
                }
                else i++;
                
            }

            return RedirectToAction("View", new { InvId = int.Parse(col["InterventionId"]) });
        }

        [HttpPost]
        public ActionResult ChangeStatus(FormCollection col) // Use this for all status changes (requires a refactoring)
        {
            int IntervId = int.Parse(col["IntervId"]);
            string Destination = col["selDest"];

            string[] DestStuff = Destination.Split(',');

            NrepServ localservice = new NrepServ(NrepServ.ConnString);

            localservice.ChangeStatus(IntervId, DestStuff[0], int.Parse(DestStuff[1]));

            return RedirectToAction("Screen", new { InterventionId = int.Parse(col["IntervId"]) });
        }

        #endregion

        #region Helper Methods

        protected void UpdateRCDocument(RCDocument doc, NrepServ localService)
        {
            localService.UpdateRCDocInfo(doc.RCDocId, doc.DocId, doc.Reference, doc.RCName, doc.PubYear);
        }

        #endregion

    }
}