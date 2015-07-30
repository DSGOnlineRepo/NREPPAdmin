using System.Data.Entity;
using NREPPAdminSite.Models;

namespace NREPPAdminSite.Context
{
    public class RFDIEntityContext : DbContext
    {
        public RFDIEntityContext()
            : base("name=LocalDev")
        {
        }
        public DbSet<ProgramDetail> programDetails { get; set; }
        public DbSet<ImplementationLocation> implemantationLocationDetails { get; set; }
        public DbSet<ImplementationTrainings> impleTraining { get; set; }
        public DbSet<PrgmSpecificDisseminationInformation> prgmDissInfo { get; set; }
        public DbSet<TypeofTrngAvailable> typeofTrngAvailable { get; set; }
        public DbSet<ProgramSpecificTechnicalAssistance> programSpecificTA { get; set; }
        public DbSet<ContinuedLrngQualityAssuranceMaterials> conLrngQltyMat { get; set; }
        public DbSet<ChkBoxAnswers> chkboxAns { get; set; }
    }
}
