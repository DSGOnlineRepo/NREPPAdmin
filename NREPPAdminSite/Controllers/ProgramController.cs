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

            theStudies = localService.GetStudiesByIntervention(InterventionId).ToList<Study>();
            //Intervention theIntervention = localService.GetInterventions()

            SqlParameter idParam = new SqlParameter() { ParameterName = "@Id", SqlDbType = SqlDbType.Int, Value = InterventionId };
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(idParam);
            List<Intervention> interventionList = localService.GetInterventions(parameters);
            theIntervention = interventionList[0];

            ScreeningModel sm = new ScreeningModel(theStudies, theIntervention);

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
        public ActionResult AddStudy(FormCollection formCollection)
        {
            //int InvId = 0;
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            Study nStudy = new Study();
            nStudy.StudyId = int.Parse(Request.Form["StudyId"]);
            nStudy.DocumentId = int.Parse(Request.Form["Document"]);
            nStudy.StudyDesign = int.Parse(Request.Form["StudyDesign"]);
            nStudy.BaselineEquiv = Request.Form["BaselineEquiv"];
            nStudy.Reference = "Some Reference";
            nStudy.Id = -1;

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