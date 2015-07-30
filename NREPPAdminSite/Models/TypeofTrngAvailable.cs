namespace NREPPAdminSite.Models
{
    public class TypeofTrngAvailable
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public int TrngId { get; set; }
        public string Nameoftrng { get; set; }
        public string Trngdeliverymethod { get; set; }
        public string Whereavailable { get; set; }
        public string AvailableLang { get; set; }
        public string RequiredTraining { get; set; }
        public string DurationTraining { get; set; }
        public string TypicalTrainingCostPerUnit { get; set; }
    }
}
