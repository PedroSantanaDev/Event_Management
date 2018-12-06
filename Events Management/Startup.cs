using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Events_Management.Startup))]
namespace Events_Management
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
