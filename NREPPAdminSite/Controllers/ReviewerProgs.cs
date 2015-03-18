using NREPPAdminSite.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace NREPPAdminSite.Controllers
{
    public partial class ProgramController : Controller
    { 
        public ActionResult ScreenResults(int IntervId)
        {
            List<Study> theStudies = new List<Study>();
            NrepServ localService = new NrepServ(NrepServ.ConnString);
            Intervention theIntervention;
            List<Answer> StudyDesigns;
            List<Answer> YPYN;
            List<Answer> Exclusions;
            List<RCDocument> reviewerDocs;



            theStudies = localService.GetStudiesByIntervention(IntervId).ToList<Study>();
            StudyDesigns = localService.GetAnswersByCategory("StudyDesign").ToList<Answer>();
            YPYN = localService.GetAnswersByCategory("YPYN").ToList<Answer>();
            Exclusions = localService.GetAnswersByCategory("Exclusions").ToList<Answer>();

            //List<Object> something = theStudies.GroupBy(s => s.StudyId).Select(group => new { StudyId = group.Key });

            //theIntervention = localService.GetInterventions(InterventionId);

            SqlParameter idParam = new SqlParameter() { ParameterName = "@Id", SqlDbType = SqlDbType.Int, Value = IntervId };
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(idParam);
            List<Intervention> interventionList = localService.GetInterventions(parameters);
            theIntervention = interventionList[0];

            OutcomesWrapper ow = localService.GetOutcomesByIntervention(IntervId);

            List<OutcomeMeasure> oms = ow.OutcomesMeasures.Where(om => om.OutcomeId == 1).ToList<OutcomeMeasure>();
            reviewerDocs = localService.GetRCDocuments(null, theIntervention.Id);

            ScreeningModel sm = new ScreeningModel(theStudies, theIntervention, StudyDesigns, YPYN, Exclusions, ow, reviewerDocs);
            sm.AddDests(localService.GetDestinations(theIntervention.Id).ToList());

            return View(sm);
        }
    }
}