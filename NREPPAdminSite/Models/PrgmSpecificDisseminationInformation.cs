namespace NREPPAdminSite.Models
{
    public class PrgmSpecificDisseminationInformation
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public int TypeofdisseminationinformationId { get; set; }
        public string NameofDissItem { get; set; }
        public string Disseminationformat { get; set; }
        public string DisseminationWhereavailable { get; set; }
        public string DisseminationAvailableLang { get; set; }
        public string DisseminationRequired { get; set; }
        public string DisseminationTypicalCostPerUnit { get; set; }
    }
}
