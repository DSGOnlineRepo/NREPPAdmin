using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using NREPPAdminSite;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace NREPPAdminSite
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            CookieAuthenticationOptions options = new CookieAuthenticationOptions();
            options.AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie;
            options.LoginPath = new PathString("/admin/login");
            app.UseCookieAuthentication(options);
        }
    }
}