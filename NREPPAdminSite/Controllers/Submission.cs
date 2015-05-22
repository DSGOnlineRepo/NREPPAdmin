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
        #region Get Methods

        public ActionResult Submission1()
        {
            return View();
        }

        public ActionResult Submission2()
        {
            IntervPageModel pageModel;
            NrepServ localService = new NrepServ(NrepServ.ConnString);

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

           
                theIntervention = new Intervention(-1, "", "", User.Identity.Name, null, DateTime.Now, User.Identity.GetUserId(), "", -1, 0, "", false);
                pageModel = new IntervPageModel();
                documentz = new List<InterventionDoc>();
                reviewerDocs = new List<RCDocument>();

            //List<Destination> nDests = localService.GetDestinations(theIntervention.Id).ToList();

            pageModel = new IntervPageModel(documentz, reviewerDocs, MaskValue.SplitMask(programTypes, theIntervention.ProgramType).ToList<MaskValue>(),
                documentTypes, null, MaskValue.SplitMask(preScreen, theIntervention.PreScreenMask).ToList<MaskValue>(),
                MaskValue.SplitMask(preScreen, theIntervention.UserPreScreenMask).ToList<MaskValue>());


            return View(pageModel);
        }

        public ActionResult ReSubmission2(int InvId)
        {
            IntervPageModel pageModel;
            NrepServ localService = new NrepServ(NrepServ.ConnString);

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

            //List<Destination> nDests = localService.GetDestinations(theIntervention.Id).ToList();

            pageModel = new IntervPageModel(documentz, reviewerDocs, MaskValue.SplitMask(programTypes, theIntervention.ProgramType).ToList<MaskValue>(),
                documentTypes, null, MaskValue.SplitMask(preScreen, theIntervention.PreScreenMask).ToList<MaskValue>(),
                MaskValue.SplitMask(preScreen, theIntervention.UserPreScreenMask).ToList<MaskValue>());

            pageModel.TheIntervention = theIntervention;

            return View("Submission2", pageModel);
        }

        public ActionResult Submission4(int InvId)
        {
            IntervPageModel pageModel;
            NrepServ localService = new NrepServ(NrepServ.ConnString);

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

            //List<Destination> nDests = localService.GetDestinations(theIntervention.Id).ToList();

            pageModel = new IntervPageModel(documentz, reviewerDocs, MaskValue.SplitMask(programTypes, theIntervention.ProgramType).ToList<MaskValue>(),
                documentTypes, null, MaskValue.SplitMask(preScreen, theIntervention.PreScreenMask).ToList<MaskValue>(),
                MaskValue.SplitMask(preScreen, theIntervention.UserPreScreenMask).ToList<MaskValue>());

            pageModel.TheIntervention = theIntervention;

            List<string> perms = new List<string>();

            perms.Add("UploadDocs");

            pageModel.SetPermissions(perms, User.Identity.Name, InvId);
            pageModel.TheIntervention = theIntervention;


            return View(pageModel);
        }

        public ActionResult Submission6(int InvId)
        {
            IntervPageModel pageModel;
            NrepServ localService = new NrepServ(NrepServ.ConnString);

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

            //List<Destination> nDests = localService.GetDestinations(theIntervention.Id).ToList();

            pageModel = new IntervPageModel(documentz, reviewerDocs, MaskValue.SplitMask(programTypes, theIntervention.ProgramType).ToList<MaskValue>(),
                documentTypes, null, MaskValue.SplitMask(preScreen, theIntervention.PreScreenMask).ToList<MaskValue>(),
                MaskValue.SplitMask(preScreen, theIntervention.UserPreScreenMask).ToList<MaskValue>());

            pageModel.TheIntervention = theIntervention;

            List<string> perms = new List<string>();

            perms.Add("UploadDocs");

            pageModel.SetPermissions(perms, User.Identity.Name, InvId);
            pageModel.TheIntervention = theIntervention;


            return View(pageModel);
        }


        public ActionResult MinReq(int InvId)
        {
            ViewBag.Id = InvId;

            return View();
        }

        #endregion

        #region Post Methods

        public ActionResult Submit2(IntervPageModel model, FormCollection col)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            model.TheIntervention.SubmitterId = User.Identity.GetUserId();
            int returnValue = localService.SaveIntervention(model.TheIntervention);
            

            int prescreenMask = model.TheIntervention.UserPreScreenMask;

            if (prescreenMask == 10 || prescreenMask == 18 || prescreenMask == 12 || prescreenMask == 20)
                return RedirectToAction("Submission4", new { InvId = returnValue }); // Do I need to do this action? I don't think so...
            else
                return RedirectToAction("MinReq", new { InvId = returnValue });
        }

        public ActionResult Submit6(IntervPageModel model, FormCollection col)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);

            model.TheIntervention.HaveMaterials = col["HaveMaterials"] == "1";
            int returnValue = localService.SaveIntervention(model.TheIntervention);

            if (col["Dest"] == "continue")
            {
                //return RedirectToAction("ConfirmSub", model.TheIntervention.Id);
                return View("ConfirmSub", model);
            }
            else return RedirectToAction("Submission6", new { InvId = model.TheIntervention.Id });
        }

        #endregion
    }
}