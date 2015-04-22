using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using NREPPAdminSite.Models;

namespace NREPPAdminSite.Security
{
    public class MyIdentityDbContext : IdentityDbContext<ExtendedUser>
    {
        public MyIdentityDbContext() : base("LocalDev")
        {
        }
    }
}
