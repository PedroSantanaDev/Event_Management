using Events.Data;

using System.Web.Mvc;

using Microsoft.AspNet.Identity;

namespace Events_Management.Controllers
{
    [ValidateInput(false)]
    public class BaseController : Controller
    {
        protected ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Check if user is an administrator
        /// </summary>
        /// <returns>boolean</returns>
        public bool IsAdmin()
        {
            var currentUserId = this.User.Identity.GetUserId();
            var isAdmin = (currentUserId != null && this.User.IsInRole("Administrator"));
            return isAdmin;
        }
    }
    
}