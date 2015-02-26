using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NREPPAdminSite.Models;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace NREPPAdminSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //NreppUser usr = ReadCookie(Request);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

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

            HttpCookie usrStuff = req.Cookies.Get(Constants.USR_COOKIE);
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
                    Request.Cookies.Remove(Constants.USR_COOKIE);
                    outUser = null;
                }
            }

            return outUser;
        }

        #endregion
    }
}