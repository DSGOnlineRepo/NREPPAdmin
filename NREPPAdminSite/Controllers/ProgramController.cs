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
                SqlParameter idParam = new SqlParameter() { ParameterName = "@Id", SqlDbType = SqlDbType.Int, Value = InvId };
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(idParam);
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

            
            
            theStudies = localService.GetStudiesByIntervention(InterventionId).ToList<Study>();
            StudyDesigns = localService.GetAnswersByCategory("StudyDesign").ToList<Answer>();
            YPYN = localService.GetAnswersByCategory("YPYN").ToList<Answer>();
            Exclusions = localService.GetAnswersByCategory("Exclusions").ToList<Answer>();

            //theIntervention = localService.GetInterventions(InterventionId);

            SqlParameter idParam = new SqlParameter() { ParameterName = "@Id", SqlDbType = SqlDbType.Int, Value = InterventionId };
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(idParam);
            List<Intervention> interventionList = localService.GetInterventions(parameters);
            theIntervention = interventionList[0];

            OutcomesWrapper ow = localService.GetOutcomesByIntervention(InterventionId);

            ScreeningModel sm = new ScreeningModel(theStudies, theIntervention, StudyDesigns, YPYN, Exclusions, ow);

            return View(sm);
        }

        #region Post Methods

        /// <summary>
        /// Performs the submission of a new program
        /// </summary>
        /// <param name="model">The intervention to submit for review</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(IntervPageModel model)
        {
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
            nStudy.DocumentId = int.Parse(Request.Form["Document"]);
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