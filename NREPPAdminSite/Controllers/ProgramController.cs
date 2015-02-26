using NREPPAdminSite.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace NREPPAdminSite.Controllers
{
    public class ProgramController : Controller
    {
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
            List<InterventionDoc> documentz;

            if (InvId > 0)
            {
                HttpCookie usrStuff = Request.Cookies.Get(Constants.USR_COOKIE);
                NreppUser outUser = (new JavaScriptSerializer()).Deserialize<NreppUser>(usrStuff.Value);

                SqlParameter idParam = new SqlParameter() { ParameterName = "@Id", SqlDbType = SqlDbType.Int, Value = InvId };
                SqlParameter roleParam = new SqlParameter() { ParameterName = "@UserRoleId", Value = 1 };
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(idParam);
                parameters.Add(roleParam);
                List<Intervention> interventionList = localService.GetInterventions(parameters);
                theIntervention = interventionList[0];

                documentz = localService.GetDocuments(InvId, null, null).ToList<InterventionDoc>();
            }
            else
            {
                theIntervention = new Intervention(-1, "", "", "", null, DateTime.Now, -1, "", -1, 0, "", false);
                pageModel = new IntervPageModel();
                documentz = new List<InterventionDoc>();
            }

            pageModel = new IntervPageModel(documentz, MaskValue.SplitMask(programTypes, theIntervention.ProgramType).ToList<MaskValue>(),
                documentTypes);
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

            
            
            theStudies = localService.GetStudiesByIntervention(InterventionId).ToList<Study>();
            StudyDesigns = localService.GetAnswersByCategory("StudyDesign").ToList<Answer>();
            YPYN = localService.GetAnswersByCategory("YPYN").ToList<Answer>();
            Exclusions = localService.GetAnswersByCategory("Exclusions").ToList<Answer>();

            //List<Object> something = theStudies.GroupBy(s => s.StudyId).Select(group => new { StudyId = group.Key });

            //theIntervention = localService.GetInterventions(InterventionId);

            SqlParameter idParam = new SqlParameter() { ParameterName = "@Id", SqlDbType = SqlDbType.Int, Value = InterventionId };
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(idParam);
            List<Intervention> interventionList = localService.GetInterventions(parameters);
            theIntervention = interventionList[0];

            OutcomesWrapper ow = localService.GetOutcomesByIntervention(InterventionId);

            List<OutcomeMeasure> oms = ow.OutcomesMeasures.Where(om => om.OutcomeId == 1).ToList<OutcomeMeasure>();
            reviewerDocs = localService.GetRCDocuments(null, theIntervention.Id);

            ScreeningModel sm = new ScreeningModel(theStudies, theIntervention, StudyDesigns, YPYN, Exclusions, ow, reviewerDocs);

            return View(sm);
        }

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

            localService.ChangeStatus(model.TheIntervention.Id, -1, int.Parse(col["destStatus"]));

            return RedirectToAction("Programs", "Home");
        }

        [HttpPost]
        public ActionResult Edit(IntervPageModel inInterv)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            NreppUser usr = ReadCookie(Request);
            inInterv.TheIntervention.SubmitterId = usr.Id;
            int returnValue = localService.SaveIntervention(inInterv.TheIntervention);

            return RedirectToAction("View", new { InvId = returnValue });
        }

        [HttpPost]
        public ActionResult Upload(FormCollection formCollection)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            HttpCookie usrStuff = Request.Cookies.Get(Constants.USR_COOKIE);
            NreppUser outUser = (new JavaScriptSerializer()).Deserialize<NreppUser>(usrStuff.Value);

            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                    localService.SaveFileToDB(fileBytes, fileName, 1, "NOT IMPLEMENTED!", int.Parse(Request.Form["TheIntervention.Id"]), false,
                        -1, Request.Form["FileDescription"], int.Parse(Request.Form["docTypeDD"])); // TODO: Add UserId to the Cookie. :|
                }
            }

            return RedirectToAction("View", new { InvId = int.Parse(Request.Form["TheIntervention.Id"]) });
        }

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
            nStudy.Reference = Request.Form["Reference"];
            nStudy.Id = -1;
            nStudy.Notes = Request.Form["Notes"];
            nStudy.UseMultivariate = Request.Form["useMultivariate"] == "on";
            nStudy.SAMSHARelated = int.Parse(Request.Form["SAMSHARelated"]);
            nStudy.Exclusion1 = int.Parse(Request.Form["Exclusion1"]);
            nStudy.Exclusion2 = int.Parse(Request.Form["Exclusion2"]);
            nStudy.Exclusion3 = int.Parse(Request.Form["Exclusion3"]);
            nStudy.Id = Request.Form["Id"] == null || Request.Form["Id"] == string.Empty ? -1 : int.Parse(Request.Form["Id"]);

            int ActualId = localService.AddStudyRecord(nStudy);

            return RedirectToAction("Screen", new { InterventionId = int.Parse(Request.Form["InterventionId"]) });
        }

        [HttpPost]
        public ActionResult AddOutcome(FormCollection col)
        {
            int IntervId = int.Parse(col["IntervId"]);
            
            OutcomeMeasure om = new OutcomeMeasure();
            om.Id = col["OutcomeMeasureId"] == string.Empty || int.Parse(col["OutcomeMeasureId"]) < 1 ? -1 : int.Parse(col["OutcomeMeasureId"]);
            om.DocumentId = int.Parse(col["docDropDown"]);
            //om.DiffAttrition = bool.Parse(col["DiffAttrition"]);
            //om.EffectSize = bool.Parse(col["EffectSize"]);
            om.PopDescription = col["popDescription"];
            om.StudyId = int.Parse(col["studySelector"]);
            om.OutcomeMeasureName = col["measure"];
            om.OutcomeId = int.Parse(col["MacroOutcome"]);

            NrepServ localService = new NrepServ(NrepServ.ConnString);
            localService.AddOrUpdateOutcomeMeasure(om, IntervId, col["newOutcome"].Trim());


            return RedirectToAction("Screen", new { InterventionId = IntervId }); // TODO: Pass errors on failure

            /*
             *  <label>Write in: </label><input type="text" name="newOutcome" /><br /><br />
        <label for="measure">Measure Reported in the Study</label>
        <input type="text" id="measure" name="measure" /><br /><br />
        
        <label for="popDescription">Describe Evaluated Population:</label><br />
        <textarea name="popDescription" id="popDescription"></textarea><br /><br />
        
        <label for="studySelector">Select Applicable Studies</label><br />
        <select id="studySelector" name="studySelector" multiple class="chosen-select" style="width: 350px;">
            @foreach(NREPPAdminSite.Models.Study study in Model.StudyDocuments)
            {
                <option value="@study.Id">Study @study.StudyId</option>
            }
        </select>
             */
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
            NrepServ localService = new NrepServ(NrepServ.ConnString);

            int i = 0;

            while(i < col.Count) // break if i gets too big
            {
                if (col[dirtyText + i.ToString()] == "true")
                {
                    // TODO: Check to see if an RCDocinfo Exists
                    localService.UpdateRCDocInfo(-1, int.Parse(col[hiddenText + i.ToString()]), col[ReferenceText + i.ToString()], col[RCNameText + i.ToString()]);
                    break;
                }
                else i++;
                
            }

            return RedirectToAction("Screen", new { InterventionId = int.Parse(col["InterventionId"]) });
        }

        #endregion

        #region Helper Methods

        protected NreppUser ReadCookie(HttpRequestBase req)
        {
            NreppUser outUser = new NreppUser();

            HttpCookie usrStuff = req.Cookies.Get(Constants.USR_COOKIE);
            //NreppUser usr;

            if (usrStuff.Value != "")
            {
                try
                {
                    outUser = (new JavaScriptSerializer()).Deserialize<NreppUser>(usrStuff.Value);
                }
                catch (Exception)
                {
                    Request.Cookies.Remove(Constants.USR_COOKIE);
                    outUser = null;
                }
            }

            return outUser;
        }

        #endregion

    }
}