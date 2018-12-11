using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Events.Data
{
  
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public IDbSet<Event> Events { get; set; }
        public IDbSet<Comment> Comments { get; set; }

    }
}