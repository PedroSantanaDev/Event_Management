using Events_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Events_Management.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            //Queries database for public events
            var events = this.db.Events
                .OrderBy(e => e.StartDateTime)
                .Where(e => e.IsPublic)
                .Select(e => new EventViewModel()
                {
                    Id = e.Id,
                    Title = e.Title,
                    StartDateTime = e.StartDateTime,
                    Duration = e.Duration,
                    Author = e.Author.FullName,
                    Location = e.Location
                });

            //Upcoming events
            var upcomingEvents = events.Where(e => e.StartDateTime > DateTime.Now);
            //Passed events
            var passedEvents = events.Where(e => e.StartDateTime <= DateTime.Now);

            return View(new UpcomingPassedEventsViewModel()
            {
                UpcomingEvents = upcomingEvents,
                PassedEvents = passedEvents
            });
            
        }
    }
}