using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using NREPPAdminSite.Constants;
using NREPPAdminSite.Models;

namespace NREPPAdminSite.Controllers
{
    public class HomeController : Controller
    {
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
            NreppUser usr = ReadCookie(Request);
            ViewBag.Fname = usr.Firstname;

            NrepServ localService = new NrepServ(NrepServ.ConnString);

            List<Intervention> interventionList = localService.GetInterventions(usr.UserRole.RoleId);


            return View(interventionList);
        }

        #region Helper Functions

        protected NreppUser ReadCookie(HttpRequestBase req)
        {
            NreppUser outUser = new NreppUser();

            HttpCookie usrStuff = req.Cookies.Get(SystemConstants.USR_COOKIE);
            //NreppUser usr;

            if (usrStuff.Value != "")
            {
                try
                {
                    outUser = (new JavaScriptSerializer()).Deserialize<NreppUser>(usrStuff.Value);
                    dynamic dyn = JsonConvert.DeserializeObject(usrStuff.Value);

                    outUser.setRole(Convert.ToInt32(dyn["UserRole"]["RoleId"]), Convert.ToString(dyn["UserRole"]["RoleName"]));
                }
                catch (Exception)
                {
                    Request.Cookies.Remove(SystemConstants.USR_COOKIE);
                    outUser = null;
                }
            }

            return outUser;
        }

        #endregion
    }
}