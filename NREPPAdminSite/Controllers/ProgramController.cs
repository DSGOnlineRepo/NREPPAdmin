using NREPPAdminSite.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

            NrepServ localService = new NrepServ(NrepServ.ConnString);
            SqlParameter idParam = new SqlParameter() { ParameterName = "@Id", SqlDbType = SqlDbType.Int, Value = InvId };
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(idParam);

            List<Intervention> interventionList = localService.GetInterventions(parameters);
            Intervention theIntervention = interventionList[0];
            return View(theIntervention);
        }

        [HttpPost]
        public ActionResult Edit(Intervention inInterv)
        {
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            localService.SaveIntervention(inInterv);

            return RedirectToAction("View", new { InvId = inInterv.Id });
        }
    }
}