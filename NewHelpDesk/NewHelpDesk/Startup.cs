using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NewHelpDesk.Startup))]
namespace NewHelpDesk
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
