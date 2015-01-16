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

                documentz = localService.GetDocuments(InvId, null).ToList<InterventionDoc>();

            }
            else
            {
                theIntervention = new Intervention(-1, "", "", "", null, DateTime.Now, -1, "", -1, 0);
                pageModel = new IntervPageModel();
                documentz = new List<InterventionDoc>();
            }

            pageModel = new IntervPageModel(documentz, MaskValue.SplitMask(programTypes, theIntervention.ProgramType).ToList<MaskValue>(),
                documentTypes);
            pageModel.TheIntervention = theIntervention;

            return View(pageModel);
        }

        [HttpPost]
        public ActionResult Edit(IntervPageModel inInterv)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            localService.SaveIntervention(inInterv.TheIntervention);

            return RedirectToAction("View", new { InvId = inInterv.TheIntervention.Id });
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
                        -1, Request.Form["FileDescription"]); // TODO: Add UserId to the Cookie. :|
                }
            }

            return RedirectToAction("View", new {InvId = int.Parse(Request.Form["TheIntervention.Id"])});
        }

        // TODO: Make sure you have a cookie and rights to delete this document (pretty easy, but you DO need to implement it)

        public ActionResult DeleteDocument(int DocId, int InvId)
        {
            return RedirectToAction("View", new { InvId = InvId });
        }

        /*[HttpPost]
        public ActionResult DeleteDoc()*/

        // Delete document with an object

        public ActionResult UploadFile()
        {
            return View();
        }

        public string Document()
        {
            /*NrepServ localService = new NrepServ(NrepServ.ConnString);

            localService.getFilePath(1);*/

            return "Something came out!";
        }
    }
}