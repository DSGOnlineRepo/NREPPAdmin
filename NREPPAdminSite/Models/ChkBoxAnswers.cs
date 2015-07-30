namespace NREPPAdminSite.Models
{
    public class ChkBoxAnswers
    {
        public int id { get; set; }
        public int ProgramId { get; set; }
        public int QuestionId { get; set; }
        public int CheckBoxId { get; set; }
        public bool IsChecked { get; set; }
        public string SpecifiedAnswer { get; set; }

    }
}
