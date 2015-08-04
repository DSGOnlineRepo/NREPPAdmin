using Microsoft.AspNet.Identity.EntityFramework;
using NREPPAdminSite.Models;

namespace NREPPAdminSite.Context
{
    public class MyIdentityDbContext : IdentityDbContext<ExtendedUser>
    {
        public MyIdentityDbContext() : base("LocalDev")
        {
        }
    }
}
