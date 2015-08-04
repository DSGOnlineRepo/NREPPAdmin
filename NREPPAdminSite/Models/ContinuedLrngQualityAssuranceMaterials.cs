namespace NREPPAdminSite.Models
{
    public class ContinuedLrngQualityAssuranceMaterials
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public int TypeofQualityAssuranceitem { get; set; }
        public string NameofQualityAssuranceItem { get; set; }
        public string QualityAssuranceformat { get; set; }
        public string QualityAssuranceWhereavailable { get; set; }
        public string QualityAssuranceAvailableLang { get; set; }
        public string QualityAssuranceRequired { get; set; }
        public string QualityAssuranceDuration { get; set; }
        public string QualityAssuranceTypicalCostPerUnit { get; set; }
    }
}
