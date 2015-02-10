using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SFLCC_Demo_Website.Startup))]
namespace SFLCC_Demo_Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
