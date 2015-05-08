using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using DataTables.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NREPPAdminSite.Models;
using NREPPAdminSite.Security;

namespace NREPPAdminSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ExtendedUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        MyIdentityDbContext db = new MyIdentityDbContext();
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
            var userRoles = _userManager.GetRoles(user.Id);
            
            NrepServ localService = new NrepServ(NrepServ.ConnString);

            List<Intervention> interventionList = localService.GetInterventions(userRoles[0]);


            return View(interventionList);
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

        public ActionResult GetUser(string id)
        {
            UserPageModel userPageModel = new UserPageModel {User = _userManager.FindById(id)};
            if (!string.IsNullOrEmpty(id)) { 
             userPageModel.UserRole = _userManager.GetRoles(id).ToList().FirstOrDefault();
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
            bool isNew = false;
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(request.User.Id))
                    {
                        isNew = true;
                        var user = new ExtendedUser();
                        request.User.Id = user.Id;
                        result = _userManager.Create(request.User, request.Password);
                    }
                    else
                    {
                        db.Entry(request.User).State = System.Data.Entity.EntityState.Modified;
                        result = _userManager.Update(request.User);
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                string sErrors = string.Empty;
                foreach (var eve in e.EntityValidationErrors)
                {
                    sErrors += eve.Entry.Entity.GetType().Name + eve.Entry.State + "\n";
                    foreach (var ve in eve.ValidationErrors)
                    {
                        sErrors += ve.PropertyName + "message:" + ve.ErrorMessage;
                    }
                }
                ModelState.AddModelError("UserName", sErrors);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("UserName", ex.Message);
            }

            if (result.Succeeded && isNew)
            {
                _userManager.AddToRole(request.User.Id, request.UserRole);               
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