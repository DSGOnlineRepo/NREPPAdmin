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
using System.Globalization;
using System.Net.Mail;
using DataTables.Mvc;
using NREPPAdminSite.Context;
using NREPPAdminSite.Utilities;

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

        [Authorize]
        public ActionResult Submission2()
        {
            IntervPageModel pageModel;
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            MyIdentityDbContext db = new MyIdentityDbContext();
            UserStore<ExtendedUser> userStore = new UserStore<ExtendedUser>(db);
            UserManager<ExtendedUser> _userManager = new UserManager<ExtendedUser>(userStore);

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

            // Fix this so that you don't need the "ReSubmission" page

            theIntervention = new Intervention(-1, "", "", User.Identity.Name, null, DateTime.Now, User.Identity.GetUserId(), "", -1, 0, "", false, User.Identity.Name);
            ExtendedUser me = _userManager.FindByName(User.Identity.Name);

            theIntervention.PrimaryName = me.FirstName + " " + me.LastName;
            theIntervention.PrimaryAddressLine1 = me.HomeAddressLine1;
            theIntervention.PrimaryAddressLine2 = me.HomeAddressLine2;
            theIntervention.PrimaryCity = me.HomeCity;
            theIntervention.PrimaryState = me.HomeState;
            theIntervention.PrimaryZip = me.HomeZip;
            theIntervention.PrimaryPhoneNumber = me.PhoneNumber;
            theIntervention.PrimaryFaxNumber = me.FaxNumber;
            theIntervention.PrimaryEmail = me.Email;

            pageModel = new IntervPageModel();
            documentz = new List<InterventionDoc>();
            reviewerDocs = new List<RCDocument>();

            //List<Destination> nDests = localService.GetDestinations(theIntervention.Id).ToList();

            pageModel = new IntervPageModel(documentz, reviewerDocs, MaskValue.SplitMask(programTypes, theIntervention.ProgramType).ToList<MaskValue>(),
                documentTypes, null, MaskValue.SplitMask(preScreen, theIntervention.PreScreenMask).ToList<MaskValue>(),
                MaskValue.SplitMask(preScreen, theIntervention.UserPreScreenMask).ToList<MaskValue>());

            pageModel.TheIntervention = theIntervention;


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
            
            theIntervention = localService.GetInterventions(parameters).Interventions[0];

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
            //IntervPageModel pageModel;
            DocUploadPage pageModel;
            NrepServ localService = new NrepServ(NrepServ.ConnString);

            List<MaskValue> intervTypez = new List<MaskValue>();

            Intervention theIntervention;

            List<Answer> documentTypes = localService.GetAnswersByCategory("DocumentType").ToList<Answer>();
            List<InterventionDoc> documentz;

            var user = _userManager.FindByName(User.Identity.Name);
            var userRoles = _userManager.GetRoles(user.Id);
            SqlParameter idParam = new SqlParameter() { ParameterName = "@Id", SqlDbType = SqlDbType.Int, Value = InvId };
            SqlParameter roleParam = new SqlParameter() { ParameterName = "@UserName", Value = User.Identity.Name };
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(idParam);
            parameters.Add(roleParam);
            theIntervention = localService.GetInterventions(parameters).Interventions[0];

            documentz = localService.GetDocuments(InvId, null, null).ToList<InterventionDoc>();

            //List<Destination> nDests = localService.GetDestinations(theIntervention.Id).ToList();

            /*pageModel = new IntervPageModel(documentz, reviewerDocs, MaskValue.SplitMask(programTypes, theIntervention.ProgramType).ToList<MaskValue>(),
                documentTypes, null, MaskValue.SplitMask(preScreen, theIntervention.PreScreenMask).ToList<MaskValue>(),
                MaskValue.SplitMask(preScreen, theIntervention.UserPreScreenMask).ToList<MaskValue>());*/

            pageModel = new DocUploadPage(documentz, documentTypes);

            pageModel.InterventionId = InvId;
            pageModel.InterventionName = theIntervention.Title;

            List<string> perms = new List<string>();

            perms.Add("UploadDocs");

            pageModel.SetPermissions(perms, User.Identity.Name, InvId);
            //pageModel.TheIntervention = theIntervention;


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
            theIntervention = localService.GetInterventions(parameters).Interventions[0];

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

        /// <summary>
        /// This is hardcoded, and therefore unideal
        /// </summary>
        /// <returns></returns>
        public ActionResult DoSubmit(int InvId)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            localService.ChangeStatus(InvId, null, 2);
            return RedirectToAction("PostSubmit");
        }

        public ActionResult PostSubmit()
        {
            return View();
        }

        public ActionResult Assignment(int InvId)
        {

            NrepServ localService = new NrepServ(NrepServ.ConnString);
            MyIdentityDbContext db = new MyIdentityDbContext();
            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(db);
            RoleManager<IdentityRole> _roleManager = new RoleManager<IdentityRole>(roleStore);
            UserStore<ExtendedUser> userStore = new UserStore<ExtendedUser>(db);
            UserManager<ExtendedUser> _userManager = new UserManager<ExtendedUser>(userStore);

            var list =_roleManager.FindByName("Lit Reviewer").Users;

            List<Destination> LitReviews = new List<Destination>();
            List<Tuple<string, string>> UserRoles = localService.GetUsersRoles(InvId).ToList();

            foreach (IdentityUserRole role in list)
            {
                var nDest = new Destination();
                var nUser = _userManager.FindById(role.UserId);
                nDest.RoleName = "Lit Reviewer";
                nDest.UserId = nUser.Id;
                nDest.UserName = nUser.FirstName + " " + nUser.LastName;
                LitReviews.Add(nDest);
            }

            List<Destination> nDests = localService.GetDestinations(InvId).ToList();
            //Intervention theInterv = localService.GetInterventions(parameters)[0];
            AssignmentPageModel model = new AssignmentPageModel(nDests, LitReviews);

            foreach (Tuple<string, string> tuple in UserRoles)
            {
                if (tuple.Item1 == "Lit Reviewer")
                {
                    model.HasReviewer = true;
                    ExtendedUser exu = _userManager.FindById(tuple.Item2);
                    model.LitReviewer = exu.FirstName + " " + exu.LastName;
                }
            }

            ViewBag.LitReviews = LitReviews;

            model.InvId = InvId;

            List<string> perms = new List<string>();

            perms.Add("AssignLitReview");

            model.SetPermissions(perms, User.Identity.Name, InvId);

            //List
            return View(model);
        }

        public ActionResult AssignReviewers(int InvId)
        {
            AssignReviewModel model = new AssignReviewModel();
            NrepServ localService = NrepServ.GetLocalService();

            List<ReviewerOnInterv> assignedReviewers = localService.GetReviewersByIntervention(InvId);

            foreach (ReviewerOnInterv rev in assignedReviewers)
            {
                model.AddAssignedReviewer(rev);
            }

            model.InterventionId = InvId;

            return View(model);
        }


        #endregion

        #region Post Methods

        [Authorize]
        public JsonResult ReviewersList([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            ReviewerSearchResult reviewerSearchResult = localService.GetReviewers(requestModel);
            return Json(new DataTablesResponse(requestModel.Draw, reviewerSearchResult.Reviewers, reviewerSearchResult.TotalSearchCount, reviewerSearchResult.TotalSearchCount), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Submit2(IntervPageModel model, FormCollection col)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            model.TheIntervention.SubmitterId = User.Identity.GetUserId();
            int returnValue = localService.SaveIntervention(model.TheIntervention);
            

            int prescreenMask = model.TheIntervention.UserPreScreenMask;

            if (prescreenMask == 26 || prescreenMask == 28)
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
                //model.Documents = 
                List<InterventionDoc> docz = localService.GetDocuments(model.TheIntervention.Id, null, null).ToList();
                foreach (InterventionDoc doc in docz)
                {
                    model.AddDocument(doc);
                }
                return View("ConfirmSub", model);
            }
            else return RedirectToAction("Submission6", new { InvId = model.TheIntervention.Id });
        }

        public ActionResult SetLitReview(FormCollection col)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            MyIdentityDbContext db = new MyIdentityDbContext();
            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(db);
            RoleManager<IdentityRole> _roleManager = new RoleManager<IdentityRole>(roleStore);

            var aRole = _roleManager.FindByName("Lit Reviewer");

            localService.AssignUser(col["LitReview"], aRole.Id, int.Parse(col["InvId"]));

            return RedirectToAction("Assignment", new { InvId = col["InvId"] });
        }

        public ActionResult Assign(FormCollection col)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);

            string valueString = col["ValueString"];
            string[] valuez = valueString.Split(';');

            int InterventionId = int.Parse(valuez[2]);
            int DestStatus = int.Parse(valuez[0]);
            string DestUser = valuez[1];
            

            localService.ChangeStatus(InterventionId, DestUser, DestStatus);

            return RedirectToAction("Programs", "Home");
        }

        [Authorize]
        [HttpPost]
        public ActionResult DocUpload(DocUploadPage model, FormCollection formCollection)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);

             HttpPostedFileBase file = Request.Files["UploadedFile"];
             if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
             {
                 string fileName = file.FileName;
                 string fileContentType = file.ContentType;
                 byte[] fileBytes = new byte[file.ContentLength];
                 file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                 localService.SaveFileToDB(fileBytes, fileName, User.Identity.Name, "NOT IMPLEMENTED!", model.InterventionId, false,
                            -1, model.UploadDescription, int.Parse(model.UploadDocType), model.UploadDocTitle);

             }
            
            return RedirectToAction("Submission4", new { InvId = model.InterventionId }); // This always goes here.
            
        }


        [HttpPost]
        public JsonResult UpdateStatus(string Id, bool Status)
        {
            var response = new NreppServiceResponseBase();
            try
            {
                var user = new ExtendedUser();
                user = _userManager.FindById(Id);
                user.LockoutEnabled = Status;
                _userManager.Update(user);
                response.ResponseSet(true, "Status updated.");
            }
            catch (System.Exception ex)
            {
                response.ResponseSet(false, ex.Message);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AssignReview(string id, string interventionId)
        {
            var response = new NreppServiceResponseBase();
             try
            {
               NrepServ localService = NrepServ.GetLocalService();
                GenerateReviewerInvitationLink(interventionId, id);
                localService.InviteReviewer(Convert.ToInt32(interventionId), id);
                response.ResponseSet(true, "Invitation Sent");
            }
            catch (System.Exception ex)
            {
                response.ResponseSet(false, ex.Message);
            }
            return Json(response,JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        [HttpPost]
        public JsonResult RemoveReviewer(string id, string interventionId)
        {
            var response = new NreppServiceResponseBase();
            try
            {
                NrepServ localService = NrepServ.GetLocalService();

                var reviewer = localService.GetReviewer(id);

                localService.UpdateReviewerStatus(Convert.ToInt32(interventionId), id, "Invitation Revoked");

                localService.RemoveAssignedReviewer(Convert.ToInt32(interventionId), id);

                response.ResponseSet(true, "Removed Assigner from Intervention");
            }
            catch (System.Exception ex)
            {
                response.ResponseSet(false, ex.Message);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult JoinProgram(string id)
        {
            string decryptedString = PasswordHash.AESDecrypt(HttpUtility.UrlDecode(id));
            string[] decryptedValues = decryptedString.Split(';');
            var interventionId = int.Parse(decryptedValues[0]);
            var reviewerId = decryptedValues[1];
            var dateValidUntil = DateTime.Parse(decryptedValues[2]);
            
            NrepServ localService = NrepServ.GetLocalService();
            JoinProgramModel jpm = new JoinProgramModel();

            TimeSpan difference = DateTime.Now - dateValidUntil;
            if (difference.Days > 5)
            {
                jpm.CanAccess = 3;
                return View(jpm);
            }
           
            List<ReviewerOnInterv> assignedReviewers = localService.GetReviewersByIntervention(interventionId);

            foreach (ReviewerOnInterv rev in assignedReviewers)
            {
                if (rev.Id == reviewerId)
                {
                    if (rev.ReviewerStatus == "Invitation Declined")
                    {
                        jpm.CanAccess = 1;
                        return View(jpm);
                    }
                    else if (rev.ReviewerStatus == "Invitation Accepted")
                    {
                        jpm.CanAccess = 2;
                        return View(jpm);
                    }
                }
            }
            
            
            Intervention interv = localService.GetIntervention(interventionId, "admin");
            List<RCDocument> docs = localService.GetRCDocuments(null, interventionId);
            jpm = new JoinProgramModel(docs, interv);
            return View(jpm);
        }

        public ActionResult GenerateReviewerInvitationLink(string interventionId, string reviewerId)
        {
            NrepServ localService = NrepServ.GetLocalService();
            var intervention = localService.GetIntervention(Convert.ToInt32(interventionId), User.Identity.Name);

            var reviewer = localService.GetReviewer(Convert.ToInt32(reviewerId));
            var user = _userManager.FindByName(User.Identity.Name);

            var programTypes = localService.GetMaskList("ProgramType").ToList<MaskValue>();
            var selectedProgramTypes = "";
            foreach (NREPPAdminSite.Models.MaskValue mask in MaskValue.SplitMask(programTypes, intervention.ProgramType))
            {
                if (mask.Selected)
                {
                    selectedProgramTypes += mask.Name + ",";
                }
            }
            selectedProgramTypes = selectedProgramTypes.Trim(',');

            string combinedString = HttpUtility.UrlEncode(PasswordHash.AESCrypt(string.Format("{0};{1};{2}", interventionId, reviewerId, DateTime.Now.AddDays(5))));
            var confirmUrl = Url.Action("JoinProgram", "Program", new { temp = combinedString }, Request.Url.Scheme);
            confirmUrl = confirmUrl.Replace("temp", "id");
            var emailService = new EmailService();

            var body = "Dear " + reviewer.FirstName + ", <br/>" +
                       "As part of the NREPP process, I am inviting you to review the program listed below. " +
                       "Please take a moment to confirm your availability to conduct this review." +
                       "<br /><br />" +
                       "<ul>" +
                       "<li>Program Name: " + intervention.Title + "</li>" +
                       "<li>Developer / Principal Contact: " + intervention.PrimaryName +
                       "</li>" +
                       "<li>Authors: " + "TBD" + "</li>" +
                       "<li>Type of intervention: " + selectedProgramTypes + "</li>" + 
                       "<li>Hours allotted for review: " + "TBD" + "</li>" +
                       "<li>Number of studies/articles/outcomes to be reviewed: " + "TBD" +
                       "</li>" +
                       "<li>o Brief program description: " + intervention.FullDescription +
                       "</li>" +
                       "</ul><br />" +
                       "<b>Please <a href='" + confirmUrl +
                       "'>Click hear</a> to notify me of your availability by close of business on " + 
                       DateTime.Now.AddDays(5).ToString("D") + ".</b><br/><br/>" +
                       "If you are available, please make sure that your Consulting Agreement is up-to-date and that you have completed the Conflict of Interest form.<br/><br/>" +
                       "I look forward to working with you throughout the review process. If you have any questions, please do not hesitate to ask.<br/><br/><br/>" +
                       "Regards,<br/><br/>" + user.FirstName + " " + user.LastName;

            var mailMessage = new MailMessage("donotreply@dsgonline.com",
                string.IsNullOrEmpty(reviewer.Email) ? reviewer.Email : reviewer.WorkEmail,
                "Intervention Status Changed", body)
            {
                IsBodyHtml = true
            };
            emailService.SendEmail(mailMessage);

            return View();
        }

        #endregion

        [Authorize]
        [HttpPost]
        public JsonResult MarkApprovedBySAMHSA(int id)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);

            localService.ChangeStatus(id, null, 7);

            return Json(new { Status = "Success", Message = "Error while creating the reviewer!" });
        }
    }
}