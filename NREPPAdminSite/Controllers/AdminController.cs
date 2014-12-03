﻿using System;
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
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            //ViewBag.SomeValue = localService.DoHash(model.Password1).Item1; // Tuple together the hash and salt because we will need both

            int retValue = localService.registerUser(model.UserName, "Patrick", "Taylor", model.Password1, 2);

            
            return View(retValue);
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
        public void Login(LoginViewModel model)
        {

        }

    }
}