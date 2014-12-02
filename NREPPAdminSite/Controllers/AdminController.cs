using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NREPPAdminSite.Models;

namespace NREPPAdminSite.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            /*RegisterViewModel nModel = new RegisterViewModel();
            ViewBag.message = "Registered Ok!";*/
            NrepServ localService = new NrepServ();
            ViewBag.SomeValue = localService.DoHash(model.Password1).Item1; // Tuple together the hash and salt because we will need both
            //ViewBag.OldPass = model.Password1;
            return View(model);
        }

        public ActionResult New()
        {
            return View();
        }
    }
}