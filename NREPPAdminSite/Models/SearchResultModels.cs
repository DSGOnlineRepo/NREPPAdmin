using System.Collections.Generic;

namespace NREPPAdminSite.Models
{

    public class InterventionSearchResult
    {
        public List<Intervention> Interventions = new List<Intervention>();
        public int TotalSearchCount;
    }
}