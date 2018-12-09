using Events.Data;
using Events_Management.Extensions;
using Events_Management.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Events_Management.Controllers
{
    [Authorize]
    public class EventsController : BaseController
    {
        // GET: Events
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.EventInputModel model)
        {
            if(model != null && this.ModelState.IsValid)
            {
                var e = new Event()
                {
                    AuthorId = this.User.Identity.GetUserId(),
                    Title = model.Title,
                    StartDateTime = model.StartDateTime,
                    Duration = model.Duration,
                    Description = model.Description,
                    Location = model.Location,
                    IsPublic = model.IsPublic
                };

                this.db.Events.Add(e);
                this.db.SaveChanges();
                this.AddNotification("Event created", NotificationType.INFO);
                return this.RedirectToAction("My");
            }
            return this.View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var eventToEdit = this.LoadEvent(id);
            if(eventToEdit == null)
            {
                this.AddNotification("Cannot edit event #" + id, NotificationType.ERROR);
                return this.RedirectToAction("My");
            }

            var model = EventInputModel.CreateFromEvent(eventToEdit);
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EventInputModel model)
        {
            var eventToEdit = this.LoadEvent(id);
            if(eventToEdit == null)
            {
                this.AddNotification("Cannot edit event #" + id, NotificationType.ERROR);
                return this.RedirectToAction("My");
            }

            if(model != null && this.ModelState.IsValid)
            {
                eventToEdit.Title = model.Title;
                eventToEdit.StartDateTime = model.StartDateTime;
                eventToEdit.Duration = model.Duration;
                eventToEdit.Description = model.Description;
                eventToEdit.Location = model.Location;
                eventToEdit.IsPublic = model.IsPublic;

                this.db.SaveChanges();
                this.AddNotification("Event Edited.", NotificationType.INFO);
                return this.RedirectToAction("My");
            }

            return this.View(model);
        }

        /// <summary>
        /// Loads an event by id
        /// </summary>
        /// <param name="id">Event id</param>
        /// <returns>An event</returns>
        private Event LoadEvent(int id)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var isAdmin = this.IsAdmin();          
            var eventToEdit = this.db.Events
                .Where(e => e.Id == id)
                .FirstOrDefault(e => e.AuthorId == currentUserId || isAdmin);

            return eventToEdit;
        }

        public ActionResult My()
        {

            string currentUserId = this.User.Identity.GetUserId();
            var events = this.db.Events
                .Where(e => e.AuthorId == currentUserId)
                .OrderBy(e => e.StartDateTime)
                .Select(EventViewModel.ViewModel);

            var upcomingEvents = events.Where(e => e.StartDateTime > DateTime.Now);
            var passedEvents = events.Where(e => e.StartDateTime <= DateTime.Now);
            return View(new UpcomingPassedEventsViewModel()
            {
                UpcomingEvents = upcomingEvents,
                PassedEvents = passedEvents
            });
        }
    }
}