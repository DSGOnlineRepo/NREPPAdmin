using System.Collections.Generic;
using System.Web.Mvc;

namespace NREPPAdminSite.Constants
{
    public static class DDLHelper
    {
        public static IList<SelectListItem> GetRoles()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();
            _result.Add(new SelectListItem { Value = "Principal Investigator", Text = "Principal Investigator" });
            _result.Add(new SelectListItem { Value = "Lit Review", Text = "Lit Review" });
            _result.Add(new SelectListItem { Value = "Review Coordinator", Text = "Review Coordinator" });
            _result.Add(new SelectListItem { Value = "DSG PRM", Text = "DSG PRM" });
            return _result;
        }
    }
}