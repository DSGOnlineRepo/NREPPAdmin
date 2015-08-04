using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NREPPAdminSite.Context;
using NREPPAdminSite.Models;

namespace NREPPAdminSite.Utilities
{
    public static class IdentityExtensions
    {
        public static bool IsInIdentityRole(this IPrincipal user, string role)
        {
            GetUserManager(); //implement this!
            return _userManager.IsInRole(user.Identity.GetUserId(), role);
        }

         private static UserManager<ExtendedUser> _userManager;

        private static void GetUserManager()
        {
            MyIdentityDbContext db = new MyIdentityDbContext();

            UserStore<ExtendedUser> userStore = new UserStore<ExtendedUser>(db);
            _userManager = new UserManager<ExtendedUser>(userStore);
        }
    }
}