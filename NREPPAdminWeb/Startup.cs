using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NREPPAdminWeb.Startup))]
namespace NREPPAdminWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
