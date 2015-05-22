using System.ComponentModel;
namespace NREPPAdminSite.Constants
{
    public class SystemConstants
    {
        public const string ENV_SETTING = "Environment";
        public const string LOCAL_ENV = "LocalDev";
        public const string REMOTE_ENV = "RemoteDev";
        public const string ERR_TABLE = "Errors";
        public const string ERR_MSG_COL = "ErrMsg";
    }

    public enum SystemRoles
    {
        [Description("DSG PRM")]
        DSGPRM,
        [Description("Principal Investigator")]
        PrincipalInvestigator,
        [Description("Reviewer")]
        Reviewer,
        [Description("Review Coordinator")]
        ReviewCoordinator,
        [Description("Assigner")]
        Assigner,
        [Description("Data Entry")]
        DataEntry,
        [Description("PreScreener")]
        PreScreener,
        [Description("Mathematica Assigner")]
        MathematicaAssigner,
        [Description("Lit Review")]
        LitReview,
        [Description("Mathematica Review Coordinator")]
        MathematicaReviewCoordinator
    }

    public struct SystemRoleNames
    {
        public const string DSGPRM = "DSG PRM";
        public const string PrincipalInvestigator = "Principal Investigator";
        public const string Reviewer = "Reviewer";
        public const string ReviewCoordinator = "Review Coordinator";
        public const string Assigner = "Assigner";

        public const string DataEntry = "DataEntry";
        public const string PreScreener = "PreScreener";
        public const string MathematicaAssigner = "Mathematica Assigner";
        public const string LitReview = "Lit Review";
        public const string MathematicaReviewCoordinator = "Mathematica Review Coordinator";
        public const string SystemAdmin = "System Admin";
    }

}