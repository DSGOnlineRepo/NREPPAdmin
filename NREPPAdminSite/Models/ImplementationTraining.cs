namespace NREPPAdminSite.Models
{
    public class ImplementationTrainings
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public int TypeofItemId { get; set; }
        public string Nameofitem { get; set; }
        public string IntendedAudience { get; set; }
        public string Whereavailable { get; set; }
        public string AvailableLaunguages { get; set; }
        public string Required { get; set; }
        public string TypicalCost { get; set; }
    }
}

