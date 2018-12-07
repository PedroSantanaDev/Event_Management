using Events.Data;
using Events.Data.Migrations;
using Microsoft.Owin;
using Owin;
using System.Data.Entity;

[assembly: OwinStartupAttribute(typeof(Events_Management.Startup))]
namespace Events_Management
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
        }
    }
}
