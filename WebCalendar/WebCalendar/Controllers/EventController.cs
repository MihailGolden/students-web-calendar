﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebCalendar.Contracts;
using WebCalendar.Hubs;
using WebCalendar.Mappers;
using WebCalendar.Models;
using Web = WebCalendar.Domain.Aggregate.Calendar;
using Ganss.XSS;


namespace WebCalendar.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private IEventService service;
        private ICalendarService calService;
        private INotificationService notifyService;
        private IOccurrenceService occurService;

        public EventController(IEventService service, ICalendarService calService, INotificationService notifyService, IOccurrenceService occurService)
        {
            this.service = service;
            this.calService = calService;
            this.notifyService = notifyService;
            this.occurService = occurService;
        }

        public void InitDropDownList(EventViewModel model)
        {
            model.Calendars = from calendar in this.calService.GetUserCalendars().AsQueryable()
                              select new SelectListItem { Text = calendar.Title, Value = calendar.ID.ToString() };
        }

        // GET: Event

        public ActionResult Index(int id)
        {
            var notifies = (from n in this.notifyService.GetNotifications
                            join e in this.service.GetEventsFromCalendar(id) on n.EventID
                            equals e.ID
                            select new Notify() { Title = e.Title, Date = e.BeginTime }).OrderBy(d => d.Date).ToList();
            NotifyTime.Instance.GetDates(notifies);
            var events = this.service.GetEventsFromCalendar(id);
            List<EventViewModel> list;
            if (events.Count() > 0)
            {
                list = DomainToModel.Map(events);
            }
            else
            {
                list = new List<EventViewModel>();
                list.Add(new EventViewModel() { CalendarID = id });
            }
            return View(list);
        }

        public ActionResult Test()
        {
            return View();
        }

        public JsonResult List()
        {
            var events = this.service.GetEvents;
            return Json(events, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListEvents(int id)
        {
            Web.Calendar cal = this.calService.GetUserCalendars().Where(c => c.ID == id).SingleOrDefault();

            if (cal == null)
                return new HttpStatusCodeResult(403);

            var events = this.service.GetEventsFromCalendar(id);

            return Json(events, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListEventsInTimePeriod(int calendarId, DateTime start, DateTime end)
        {
            Web.Calendar cal = this.calService.GetUserCalendars().Where(c => c.ID == calendarId).SingleOrDefault();

            if (cal == null)
                return new HttpStatusCodeResult(403);

            var events = this.service.GetEventsFromCalendar(calendarId).Where(e => e.BeginTime <= end && e.EndTime >= start);

            return Json(events, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create(int calendarID)
        {
            Web.Calendar cal = this.calService.GetUserCalendars().Where(c => c.ID == calendarID).SingleOrDefault();

            if (cal == null)
                return new HttpStatusCodeResult(403);

            EventViewModel model = new EventViewModel();
            InitDropDownList(model);
            model.BeginTime = DateTime.Now;
            model.EndTime = DateTime.Now;
            model.CalendarID = calendarID;
            return View(model);
        }

        public ActionResult CreateEvent()
        {
            EventViewModel model = new EventViewModel();
            InitDropDownList(model);
            model.BeginTime = DateTime.Now;
            model.EndTime = DateTime.Now;
            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Create(EventViewModel ev)
        {
            var sanitizer = new HtmlSanitizer();
            if (!String.IsNullOrWhiteSpace(ev.Title))
            {
                ev.Title = sanitizer.Sanitize(ev.Title);
            }
            if (!String.IsNullOrWhiteSpace(ev.Description))
            {
                ev.Description = sanitizer.Sanitize(ev.Description);
            }

            Web.Calendar cal = this.calService.GetUserCalendars().Where(c => c.ID == ev.CalendarID).SingleOrDefault();

            if (cal == null)
                return new HttpStatusCodeResult(403);

            if (ModelState.IsValid)
            {
                if (ev != null)
                {
                    if (ev.Repeat)
                    {
                        this.occurService.Create(new Domain.Aggregate.Occurrence.Occurrence() { Count = 1 });
                        var last = this.occurService.GetOccurrences.OrderByDescending(o => o.ID).Take(1).Single();
                        ev.OccurrenceID = last.ID;
                    }
                    var domain = DomainToModel.Map(ev);
                    this.service.Create(domain);
                    if (ev.Notifications.Count > 0)
                    {
                        ev.Notifications[0].EventID = this.service.GetEvents.LastOrDefault().ID;
                        this.notifyService.Create(DomainToModel.Map(ev.Notifications[0]));
                    }
                }
                return RedirectToAction("Index", new { id = ev.CalendarID });
            }
            return View(ev);
        }

        public ActionResult Update(int id)
        {
            var ev = this.service.Get(id);

            Web.Calendar cal = this.calService.GetUserCalendars().Where(c => c.ID == ev.CalendarID).SingleOrDefault();

            if (cal == null)
                return new HttpStatusCodeResult(403);

            var model = DomainToModel.Map(ev);
            InitDropDownList(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EventViewModel ev)
        {
            InitDropDownList(ev);

            return View("Create", ev);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(EventViewModel ev)
        {
            var sanitizer = new HtmlSanitizer();
            if (!String.IsNullOrWhiteSpace(ev.Title))
            {
                ev.Title = sanitizer.Sanitize(ev.Title);
            }
            if (!String.IsNullOrWhiteSpace(ev.Description))
            {
                ev.Description = sanitizer.Sanitize(ev.Description);
            }

            Web.Calendar cal = this.calService.GetUserCalendars().Where(c => c.ID == ev.CalendarID).SingleOrDefault();

            if (cal == null)
                return new HttpStatusCodeResult(403);

            if (ModelState.IsValid)
            {
                if (ev.Repeat)
                {
                    this.occurService.Create(new Domain.Aggregate.Occurrence.Occurrence() { Count = 1 });
                    var last = this.occurService.GetOccurrences.OrderByDescending(o => o.ID).Take(1).Single();
                    ev.OccurrenceID = last.ID;
                }
                if (!ev.Repeat && ev.OccurrenceID.HasValue)
                {
                    int id = ev.OccurrenceID.Value;
                    this.occurService.Delete(id);
                }
                if (ev.Notifications.Count > 0)
                {
                    ev.Notifications[0].EventID = this.service.GetEvents.LastOrDefault().ID;
                    this.notifyService.Create(DomainToModel.Map(ev.Notifications[0]));
                }
                int calendarID = ev.CalendarID;
                var domain = DomainToModel.Map(ev);
                this.service.Update(domain);
                return RedirectToAction("Index", new { id = calendarID });
            }
            return View(ev);
        }

        public ActionResult Delete(int? id)
        {
            var ev = this.service.Get(id);

            Web.Calendar cal = this.calService.GetUserCalendars().Where(c => c.ID == ev.CalendarID).SingleOrDefault();

            if (cal == null)
                return new HttpStatusCodeResult(403);

            var model = DomainToModel.Map(ev);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(EventViewModel ev)
        {
            Web.Calendar cal = this.calService.GetUserCalendars().Where(c => c.ID == ev.CalendarID).SingleOrDefault();

            if (cal == null)
                return new HttpStatusCodeResult(403);

            int calendarID = ev.CalendarID;
            var notify = this.notifyService.GetNotificationFromEvent(ev.ID);
            if (notify != null)
            {
                this.notifyService.Delete(notify.ID);
            }
            if (ev.OccurrenceID != null)
            {
                int id = ev.OccurrenceID.Value;
                this.occurService.Delete(id);
            }
            this.service.Delete(ev.ID);
            return RedirectToAction("Index", new { id = calendarID });
        }

        public ActionResult Details(int id)
        {
            var domain = this.service.Get(id);

            Web.Calendar cal = this.calService.GetUserCalendars().Where(c => c.ID == domain.CalendarID).SingleOrDefault();

            if (cal == null)
                return new HttpStatusCodeResult(403);

            var model = DomainToModel.Map(domain);
            return View(model);
        }

        public ActionResult CreateNotification(int id)
        {
            var notify = new NotificationViewModel();
            notify.ID = id;
            notify.FieldPrefix = "Notifications[" + id + "]";
            return PartialView("_CreateNotification", notify);
        }
    }
}