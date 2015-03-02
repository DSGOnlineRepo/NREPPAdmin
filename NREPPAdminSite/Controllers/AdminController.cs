﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NREPPAdminSite.Models;
using System.Data;

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
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            //ViewBag.SomeValue = localService.DoHash(model.Password1).Item1; // Tuple together the hash and salt because we will need both

            int retValue = localService.registerUser(model.UserName, model.Firstname, model.Lastname, model.Password1, 3);


            return RedirectToAction("Login");
        }

        public ActionResult New()
        {
            return View();
        }

        // GET: Login Page
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            /*DataSet uds = localService.LoginUser(model.UserName, model.Password); // This step needs to move into the service.

            NreppUser oUser = new NreppUser(1, uds.Tables[0].Rows[0]["Username"].ToString(), "Patrick", "Taylor"); // This should actually go into the session.*/
            //HttpContext.User = localService.LoginComplete(model.UserName, model.Password);

            NreppUser oUser = localService.LoginComplete(model.UserName, model.Password);
            string someJSON = oUser.MakeJSON();
            HttpCookie loginCookie = new HttpCookie(Constants.USR_COOKIE, someJSON);

            //loginCookie.Expires = DateTime.Now.AddHours(1d);
            Response.Cookies.Add(loginCookie);

            return RedirectToAction("Programs", "Home");
        }

        public ActionResult Logout()
        {
            Request.Cookies.Remove(Constants.USR_COOKIE);

            return RedirectToAction("Login");
        }

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

    }
}