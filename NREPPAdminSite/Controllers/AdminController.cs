using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using NREPPAdminSite.Constants;
using NREPPAdminSite.Models;
using NREPPAdminSite.Security;

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
            ViewBag.Roles = new SelectList(DDLHelper.GetRoles(), "Value", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ExtendedUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber
                };

                IdentityResult result = _userManager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    _userManager.AddToRole(user.Id, model.Role);
                    return RedirectToAction("Login", "Admin");
                }
                else
                {
                    ModelState.AddModelError("UserName", "Error while creating the user!");
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

        //public async Task RegisterAdmin()
        //{
        //    IdentityUser admin = await UserManager<ExtendedUser>.FindByNameAsync(UserConstants.Admin);

        //    if (admin == null)
        //    {
        //        if (!_roleManager.RoleExists(UserConstants.Admin))
        //        {
        //            _roleManager.Create(new IdentityRole(UserConstants.Admin));
        //        }

        //        if (!_roleManager.RoleExists(UserConstants.Guest))
        //        {
        //            _roleManager.Create(new IdentityRole(UserConstants.Guest));
        //        }

        //        var user = new ExtendedUser(UserConstants.Admin, string.Empty, UserConstants.Admin,
        //            UserConstants.Admin, false);

        //        var adminResult = _userManager.Create(user, UserConstants.Password);

        //        if (adminResult.Succeeded)
        //        {
        //            _userManager.AddToRole(user.Id, UserConstants.Admin);
        //        }
        //    }
        //}

    }
}