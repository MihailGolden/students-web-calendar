using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebCalendar.Contracts;
using WebCalendar.Hubs;
using WebCalendar.Mappers;
using WebCalendar.Models;

namespace WebCalendar.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        IEventService service;
        ICalendarService calService;
        INotificationService notifyService;

        public EventController(IEventService service, ICalendarService calService, INotificationService notifyService)
        {
            this.service = service;
            this.calService = calService;
            this.notifyService = notifyService;
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
            List<EventViewModel> list = DomainToModel.Map(events);
            return View(list);
        }

        public ActionResult Test()
        {
            return View();
        }

        public JsonResult ListEvents(int id)
        {
            var events = this.service.GetEventsFromCalendar(id);
            return Json(events, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListEventsInTimePeriod(int calendarId, DateTime start, DateTime end)
        {
            var events = this.service.GetEventsFromCalendar(calendarId).Where(e => e.BeginTime >= start && e.EndTime <= end);
            return Json(events, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            EventViewModel model = new EventViewModel();
            InitDropDownList(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventViewModel ev)
        {
            if (ModelState.IsValid)
            {
                if (ev != null)
                {
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
            var model = DomainToModel.Map(ev);
            InitDropDownList(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(EventViewModel ev)
        {
            if (ModelState.IsValid)
            {
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
            var model = DomainToModel.Map(ev);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(EventViewModel ev)
        {
            int calendarID = ev.CalendarID;
            var notify = this.notifyService.GetNotificationFromEvent(ev.ID);
            if (notify != null)
            {
                this.notifyService.Delete(notify.ID);
            }
            this.service.Delete(ev.ID);
            return RedirectToAction("Index", new { id = calendarID });
        }

        public ActionResult Details(int id)
        {
            var domain = this.service.Get(id);
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