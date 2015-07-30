namespace NREPPAdminSite.Models
{
    public class ProgramSpecificTechnicalAssistance
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public int TATypeId { get; set; }
        public string NameofTA { get; set; }
        public string TAdeliverymethod { get; set; }
        public string Accessibleformats { get; set; }
        public string RequiredTA { get; set; }
        public string Duration { get; set; }
        public string TypicalCostPerUnit { get; set; }
    }
}
