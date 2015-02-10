using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SFLCC_Web_Role.Startup))]
namespace SFLCC_Web_Role
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
