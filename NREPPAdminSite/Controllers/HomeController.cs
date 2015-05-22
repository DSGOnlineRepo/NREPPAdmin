using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using DataTables.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NREPPAdminSite.Models;
using NREPPAdminSite.Security;
using NREPPAdminSite.Utilities;

namespace NREPPAdminSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ExtendedUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        MyIdentityDbContext db = new MyIdentityDbContext();

        private readonly IEmailService _emailService = new EmailService();

        public HomeController()
        {
            UserStore<ExtendedUser> userStore = new UserStore<ExtendedUser>(db);
            _userManager = new UserManager<ExtendedUser>(userStore);

            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(db);
            _roleManager = new RoleManager<IdentityRole>(roleStore);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            //NreppUser usr = ReadCookie(Request);
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Programs", "Home");
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        [Authorize]
        public ActionResult Programs()
        {
            ViewBag.Message = "Some Message Here";
            var user = _userManager.FindByName(User.Identity.Name);
            
            //var userRoles = _userManager.GetRoles(user.Id);
            
            //NrepServ localService = new NrepServ(NrepServ.ConnString);

            //List<Intervention> interventionList = localService.GetInterventions(userRoles[0]);
            //return View(interventionList);
            ViewBag.Fname = user.FirstName;
            return View();
        }

        [Authorize]
        public JsonResult ProgramsList([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            var user = _userManager.FindByName(User.Identity.Name);
            var userRoles = _userManager.GetRoles(user.Id);
            List<Intervention> programsList = localService.GetInterventions(requestModel, User.Identity.Name);
            return Json(new DataTablesResponse(requestModel.Draw, programsList, programsList.Count, programsList.Count), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Users()
        {
            return View();
        }

        [Authorize]
        public JsonResult UsersList([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            UsersSearchResult usersSearchResult = localService.GetUsers(requestModel);
            return Json(new DataTablesResponse(requestModel.Draw, usersSearchResult.UserList, usersSearchResult.TotalSearchCount, usersSearchResult.TotalSearchCount), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetUser(string id)
        {
            UserPageModel userPageModel = new UserPageModel {User = _userManager.FindById(id)};
            if (!string.IsNullOrEmpty(id) && userPageModel != null)
            { 
             userPageModel.UserRole = _userManager.GetRoles(id).ToList().FirstOrDefault();
             userPageModel.UserName = userPageModel.User.UserName;
            }
            userPageModel.Roles = _roleManager.Roles.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Name
            });
            return PartialView("_AddUser",userPageModel);
        }

         [Authorize]
        public JsonResult SaveUser(UserPageModel request)
        {
            IdentityResult result= new IdentityResult();
            bool isNew = true;
            try
            {                
                if (!string.IsNullOrEmpty(request.User.Id))
                {
                    isNew = false;
                    ModelState.Remove("Password");//for Update user
                }
                if (ModelState.IsValid)
                {
                    if (isNew)
                    {
                        var user = new ExtendedUser();
                        request.User.Id = user.Id;
                        request.User.UserName = request.UserName;
                        result = _userManager.Create(request.User, request.Password);
                    }
                    else
                    {
                        var user = _userManager.FindById(request.User.Id);
                        user.FirstName = request.User.FirstName;
                        user.LastName = request.User.LastName;
                        user.UserName = request.UserName;
                        user.Email = request.User.Email;
                        user.HomeAddressLine1 = request.User.HomeAddressLine1;
                        user.HomeAddressLine2 = request.User.HomeAddressLine2;
                        user.HomeCity = request.User.HomeCity;
                        user.HomeState = request.User.HomeState;
                        user.HomeZip = request.User.HomeZip;
                        user.PhoneNumber = request.User.PhoneNumber;
                        user.FaxNumber = request.User.FaxNumber;
                        user.Employer = request.User.Employer;
                        user.Department = request.User.Department;
                        user.WorkAddressLine1 = request.User.WorkAddressLine1;
                        user.WorkAddressLine2 = request.User.WorkAddressLine2;
                        user.WorkCity = request.User.WorkCity;
                        user.WorkState = request.User.WorkState;
                        user.WorkZip = request.User.WorkZip;
                        user.WorkPhoneNumber = request.User.WorkPhoneNumber;
                        user.WorkFaxNumber = request.User.WorkFaxNumber;
                        user.WorkEmail = request.User.WorkEmail;
                        user.TwoFactorEnabled = request.User.TwoFactorEnabled;
                        
                        result = _userManager.Update(user);
                    }
                }
            }           
            catch(Exception ex)
            {
                ModelState.AddModelError("UserName", ex.Message);
            }

            if (result.Succeeded && isNew)
            {
                _userManager.AddToRole(request.User.Id, request.UserRole);   
                var mailMessage = new MailMessage("donotreply@dsgonline.com", request.User.Email, "User Account Created", "Dear " + request.User.FirstName +
                "A User account has been created for you for the NREPPAdmin site. Your user name is " + request.UserName + " and password is " + request.Password);
                _emailService.SendEmail(mailMessage);

            }
            else if (result.Succeeded && !isNew)
            {
                var oldRoleName = _userManager.GetRoles(request.User.Id).ToList().FirstOrDefault();
                if (oldRoleName != request.UserRole)
                {
                    _userManager.RemoveFromRole(request.User.Id, oldRoleName);
                    _userManager.AddToRole(request.User.Id, request.UserRole);                       
                }
            }
            else
            {
                ModelState.AddModelError("UserName", "Error while creating the user!");
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
                    ModelState.AddModelError("", errors);
                }                
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateStatus(string Id,bool Status)
        {
            var response = new NreppServiceResponseBase();
            try
            {
                var user = new ExtendedUser();
                user = _userManager.FindById(Id);
                user.LockoutEnabled =Status;
                _userManager.Update(user);
                response.ResponseSet(true, "Status updated.");
            }
            catch (System.Exception ex)
            {
                response.ResponseSet(false, ex.Message);
            }

            return Json(response,JsonRequestBehavior.AllowGet);
        }
      



    }
}