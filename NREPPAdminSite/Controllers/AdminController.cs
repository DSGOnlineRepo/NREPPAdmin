using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using NREPPAdminSite.Models;
using NREPPAdminSite.Security;
using NREPPAdminSite.Utilities;
using System.Net.Mail;

namespace NREPPAdminSite.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ExtendedUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController()
        {
            MyIdentityDbContext db = new MyIdentityDbContext();

            UserStore<ExtendedUser> userStore = new UserStore<ExtendedUser>(db);
            _userManager = new UserManager<ExtendedUser>(userStore);

            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(db);
            _roleManager = new RoleManager<IdentityRole>(roleStore);

        }

        // GET: Admin
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        #region Post Methods

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            RegisterViewModel modal = new RegisterViewModel();
            modal.CapImage = "data:image/png;base64," + Convert.ToBase64String(new CaptchaUtil().VerificationTextGenerator());
            modal.CapImageText = Convert.ToString(Session["Captcha"]);
            return View(modal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            ModelState.Remove("Role");
            model.Role = "Principal Investigator";
            if (ModelState.IsValid)
            {
                var user = new ExtendedUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    LockoutEnabled = false,
                    LockoutEndDateUtc = DateTime.UtcNow.AddYears(100),
                    HomeAddressLine1 = model.HomeAddressLine1,
                    HomeAddressLine2 = model.HomeAddressLine2,
                    HomeCity = model.HomeCity,
                    HomeState = model.HomeState,
                    HomeZip = model.HomeZip,
                    PhoneNumber = model.PhoneNumber,
                    FaxNumber = model.FaxNumber,
                    Employer = model.Employer,
                    Department = model.Department,
                    WorkAddressLine1 = model.WorkAddressLine1,
                    WorkAddressLine2 = model.WorkAddressLine2,
                    WorkCity = model.WorkCity,
                    WorkState = model.WorkState,
                    WorkZip = model.WorkZip,
                    WorkPhoneNumber = model.WorkPhoneNumber,
                    WorkFaxNumber = model.WorkFaxNumber,
                    WorkEmail = model.WorkEmail
                };

                IdentityResult result = _userManager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    _userManager.AddToRole(user.Id, model.Role);
                    return RedirectToAction("Login", "Admin");
                }
                else
                {
                    var isDuplicateUser = false;
                    foreach (string error in result.Errors)
                    {
                        if (error.Contains("is already taken."))
                        {
                            ModelState.AddModelError("UserName", error);
                            isDuplicateUser = true;
                        }
                    }
                    if (!isDuplicateUser)
                    {
                        var errors = string.Join("<br />", result.Errors);

                        IEmailService _emailService = new EmailService();
                        var mailMessage = new MailMessage("donotreply@dsgonline.com", model.Email,
                            "User Account Created", "Dear " + model.FirstName + ", " +
                            "Thank you for creating an account with the National Registry of Evidence-based Programs and Practices (NREPP)." +
                            " Your Username Is: " + model.UserName + ". Questions regarding your submission or account can be submitted to nreppadmin@dsgonline.com.");
                        _emailService.SendEmail(mailMessage);

                        ModelState.AddModelError("", errors);
                    }
                }
            }
            return View(model);
        }

        // GET: Login Page
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {

                ExtendedUser user = _userManager.Find(model.UserName, model.Password);
                if (user != null)
                {
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    ClaimsIdentity identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationProperties props = new AuthenticationProperties();
                    props.IsPersistent = model.RememberMe;
                    if (!_userManager.GetLockoutEnabled(user.Id))
                    {
                        authenticationManager.SignIn(props, identity);
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Programs", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Account Locked. Please contact the administrator");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            return View(model);
        }

        #endregion

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Login", "Admin");
        }

        // GET: Profile Page
        [HttpGet]
        [Authorize]
        public ActionResult Profile()
        {
            ExtendedUser user = _userManager.FindByName(HttpContext.User.Identity.Name);

            UserProfileModel model = new UserProfileModel();
            model.UserName = HttpContext.User.Identity.Name;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Email = user.Email;
            model.PhoneNumber = user.PhoneNumber;

            return View(model);
        }

        // POST: Profile Page
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Profile(UserProfileModel model)
        {

            if (ModelState.IsValid)
            {
                ExtendedUser user = _userManager.FindByName(HttpContext.User.Identity.Name);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;

                IdentityResult result = _userManager.Update(user);
                if (result.Succeeded)
                {
                    ViewBag.Message = "Profile updated successfully.";
                }
                else
                {
                    ModelState.AddModelError("", "Error while updating profile.");
                }
            }
            return View(model);
        }
        // GET: Change Password Page
        [HttpGet]
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: Change Password Page
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {

            if (ModelState.IsValid)
            {
                ExtendedUser user = _userManager.FindByName(HttpContext.User.Identity.Name);
                IdentityResult result = _userManager.ChangePassword(user.Id, model.CurrentPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignOut();
                    return RedirectToAction("Login", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Error while changing the password.");
                }
            }
            return View(model);
        }


        [Authorize]
        public FilePathResult GetFile(int FileId)
        {

            string MIMEType = "application/unknown";

            NrepServ localService = new NrepServ(NrepServ.ConnString);
            InterventionDoc doc = localService.GetDocuments(null, null, FileId).First();

            return File(doc.Link, MIMEType, Path.GetFileName(doc.Link));
        }

        /// <summary>
        /// Used for the Lookups editing page
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Lookups()
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            List<Answer> answerList = localService.GetAnswersByCategory("Y/N/NR").ToList<Answer>();

            List<MaskValue> SomeMasks = new List<MaskValue>();
            SomeMasks.Add(new MaskValue() { Name = "One", Value = 1 });
            SomeMasks.Add(new MaskValue() { Name = "Two", Value = 2 });
            SomeMasks.Add(new MaskValue() { Name = "Three", Value = 3 });
            SomeMasks.Add(new MaskValue() { Name = "Four", Value = 4 });


            List<MaskValue> SomeList2 = MaskValue.SplitMask(SomeMasks, 19).ToList<MaskValue>();

            LookupPageModel model = new LookupPageModel(answerList, null, SomeList2);

            return View(model);
        }
       
        [Authorize]
        public ActionResult SubmissionPeriods()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult GetSubmissionPeriods()
        {

            NrepServ localService = new NrepServ(NrepServ.ConnString);
            return null;

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult InvitationResponse(FormCollection col)
        {
            string action = Request.Form["ReviewerResponse"];

            string ReviewerId = col["ReviewerId"];
            string declineReason = col["reason"];
            int InterventionId = int.Parse(col["InterventionId"]);
            NrepServ localService = NrepServ.GetLocalService();

            if (action == "Accept")
            {
                localService.AssignReviewer(InterventionId, ReviewerId, "Invitation Accepted");
                var reviewer = localService.GetReviewer(ReviewerId);
                
                var aRole = _roleManager.FindByName("Reviewer");
                localService.AssignUser(reviewer.UserId, aRole.Id, InterventionId);
                string combinedString = HttpUtility.UrlEncode(PasswordHash.AESCrypt(string.Format("{0};{1};{2}", InterventionId, ReviewerId, DateTime.Now.AddDays(1))));
                return RedirectToAction("JoinProgram", "Program", new { id = combinedString });
            }
            else
            {
                localService.AssignReviewer(InterventionId, ReviewerId, "Invitation Declined");
            }

            return View();
        }
        
        [Authorize]
        public ActionResult Reviewers()
        {
            return View();
        }

        [Authorize]
        public ActionResult GetReviewer(string id)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            RegisterViewModel reviewer = localService.GetReviewer(id);
            return PartialView("Reviewer", reviewer);
        }

        [HttpPost]
        [Authorize]
        public JsonResult ReviewersList([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            ReviewerSearchResult reviewerSearchResult = localService.GetReviewers(requestModel);
            return Json(new DataTablesResponse(requestModel.Draw, reviewerSearchResult.Reviewers, reviewerSearchResult.TotalSearchCount, reviewerSearchResult.TotalSearchCount), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public JsonResult SaveReviewer(RegisterViewModel model)
        {
            ModelState.Remove("Role");
            ModelState.Remove("CaptchaCodeText");

            HttpPostedFileBase file = Request.Files[0];

            if (string.IsNullOrEmpty(model.Id))
            {
                ModelState.Remove("ConfirmEmail");
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
            }

            model.Role = "Reviewer";

            NrepServ localService = new NrepServ(NrepServ.ConnString);
            if (ModelState.IsValid && string.IsNullOrEmpty(model.Id))
            {
                var user = new ExtendedUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    LockoutEnabled = false,
                    LockoutEndDateUtc = DateTime.UtcNow.AddYears(100),
                    HomeAddressLine1 = model.HomeAddressLine1,
                    HomeAddressLine2 = model.HomeAddressLine2,
                    HomeCity = model.HomeCity,
                    HomeState = model.HomeState,
                    HomeZip = model.HomeZip,
                    PhoneNumber = model.PhoneNumber,
                    FaxNumber = model.FaxNumber,
                    Employer = model.Employer,
                    Department = model.Department,
                    WorkAddressLine1 = model.WorkAddressLine1,
                    WorkAddressLine2 = model.WorkAddressLine2,
                    WorkCity = model.WorkCity,
                    WorkState = model.WorkState,
                    WorkZip = model.WorkZip,
                    WorkPhoneNumber = model.WorkPhoneNumber,
                    WorkFaxNumber = model.WorkFaxNumber,
                    WorkEmail = model.WorkEmail
                };
                if (model.UserId == null)
                {
                    IdentityResult result = _userManager.Create(user, model.Password);

                    if (result.Succeeded)
                    {
                        _userManager.AddToRole(user.Id, model.Role);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "Error while creating the user!");
                        return Json(new {Status = "Success", Message = "Error while creating the user!"});
                    }

                    model.UserId = user.Id;
                    model.CreatedBy = User.Identity.Name;
                }

                model.ModifiedBy = User.Identity.Name;

                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                    localService.SaveFileToDB(fileBytes, fileName, User.Identity.Name, "NOT IMPLEMENTED!", null, false,
                        -1, "Contract Agreement", 0, "Contract Agreement");
                }
                else
                {
                    model.ContractEndDate = null;
                }

                if (localService.AddReviewer(model))
                {
                    return Json(new { Status = "Success", Message = "Reviewer created successfully" });
                }
                else
                {
                    DeleteUserById(user.Id);
                    return Json(new { Status = "Success", Message = "Error while creating the reviewer!" });
                }

                
            }
            else
            {
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                    localService.SaveFileToDB(fileBytes, fileName, User.Identity.Name, "NOT IMPLEMENTED!", null, false,
                        -1, "Contract Agreement", 0, "Contract Agreement");
                }
                else
                {
                    model.ContractEndDate = null;
                }

                model.ModifiedBy = HttpContext.User.Identity.GetUserId();
                model.ModifiedOn = DateTime.Now;
                localService.UpdateReviewer(model);
                return Json(new { Status = "Success", Message = "Reviewer updated successfully" });
            }
            return Json(new { Status = "Error", Message = "Error while creating the reviewer!" });
        }


        private void DeleteUserById(string id)
        {
            if (id != null)
            {
                var user = _userManager.FindById(id);
                var logins = user.Logins;

                foreach (var login in logins.ToList())
                {
                    _userManager.RemoveLogin(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                }

                var rolesForUser = _userManager.GetRoles(id);

                if (rolesForUser.Any())
                {
                    foreach (var item in rolesForUser.ToList())
                    {
                        _userManager.RemoveFromRole(user.Id, item);
                    }
                }

                _userManager.Delete(user);
            }
        }

        public JsonResult Captcha()
        {
            var result = new { CapImage = "data:image/png;base64," + Convert.ToBase64String(new CaptchaUtil().VerificationTextGenerator()), CapImageText = Convert.ToString(Session["Captcha"]) };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }

    public class TempClass
    {
        public bool CanView { get; set; }
        public int InterventionId { get; set; }
    }
}