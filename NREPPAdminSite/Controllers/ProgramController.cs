using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NREPPAdminSite.Controllers
{
    public class ProgramController : Controller
    {
        // GET: Program
        public ActionResult View(int InvId)
        {
            ViewBag.Title = "Program Title";
            ViewBag.Id = InvId;
            return View();
        }
    }
}