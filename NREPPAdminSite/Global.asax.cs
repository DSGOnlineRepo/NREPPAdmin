using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NREPPAdminSite.Security;

namespace NREPPAdminSite
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            MyIdentityDbContext db = new MyIdentityDbContext();
            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(db);
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(roleStore);

            if (!roleManager.RoleExists("Administrator"))
            {
                IdentityRole newRole = new IdentityRole("Administrator");
                roleManager.Create(newRole);
            }

            if (!roleManager.RoleExists("Principal Investigator"))
            {
                IdentityRole newRole = new IdentityRole("Principal Investigator");
                roleManager.Create(newRole);
            }

            if (!roleManager.RoleExists("Data Entry"))
            {
                IdentityRole newRole = new IdentityRole("Data Entry");
                roleManager.Create(newRole);
            }

            if (!roleManager.RoleExists("Principal Investigator"))
            {
                IdentityRole newRole = new IdentityRole("Principal Investigator");
                roleManager.Create(newRole);
            }

            if (!roleManager.RoleExists("Assigner"))
            {
                IdentityRole newRole = new IdentityRole("Assigner");
                roleManager.Create(newRole);
            }

            if (!roleManager.RoleExists("Lit Review"))
            {
                IdentityRole newRole = new IdentityRole("Lit Review");
                roleManager.Create(newRole);
            }

            if (!roleManager.RoleExists("Review Coordinator"))
            {
                IdentityRole newRole = new IdentityRole("Review Coordinator");
                roleManager.Create(newRole);
            }

            if (!roleManager.RoleExists("DSG PRM"))
            {
                IdentityRole newRole = new IdentityRole("DSG PRM");
                roleManager.Create(newRole);
            }

            if (!roleManager.RoleExists("Reviewer"))
            {
                IdentityRole newRole = new IdentityRole("Reviewer");
                roleManager.Create(newRole);
            }

            if (!roleManager.RoleExists("Mathematica Assigner"))
            {
                IdentityRole newRole = new IdentityRole("Mathematica Assigner");
                roleManager.Create(newRole);
            }
        }
    }
}
