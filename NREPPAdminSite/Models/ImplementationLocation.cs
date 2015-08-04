namespace NREPPAdminSite.Models
{
    public class ImplementationLocation
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public string NumberofIndividuals { get; set; }
        public string ProgramLocations { get; set; }
        public string ProgramStartedYear { get; set; }
        public string ActiveInLocation { get; set; }
    }
}
