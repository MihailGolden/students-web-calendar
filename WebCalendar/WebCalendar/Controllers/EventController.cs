using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebCalendar.Contracts;
using WebCalendar.Mappers;
using WebCalendar.Models;

namespace WebCalendar.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        IEventService service;
        ICalendarService calService;

        public EventController(IEventService service, ICalendarService calService)
        {
            this.service = service;
            this.calService = calService;
        }

        public void InitDropDownList(EventViewModel model)
        {
            model.Calendars = from calendar in this.calService.GetUserCalendars().AsQueryable()
                              select new SelectListItem { Text = calendar.Title, Value = calendar.ID.ToString() };
        }

        // GET: Event
        public ActionResult Index(int id)
        {
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

        public JsonResult ListEvents(int id, DateTime start, DateTime end)
        {
            var events = this.service.GetEventsFromCalendar(id).Where(e => e.BeginTime == start && e.EndTime == end);
            return Json(events, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            EventViewModel model = new EventViewModel();
            InitDropDownList(model);
            return View(model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(EventViewModel ev)
        {
            if (ev != null)
            {
                var domain = DomainToModel.Map(ev);
                this.service.Create(domain);
            }
            return RedirectToAction("Index", new { id = ev.CalendarID });
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
            int calendarID = ev.CalendarID;
            var domain = DomainToModel.Map(ev);
            this.service.Update(domain);
            return RedirectToAction("Index", new { id = calendarID });
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
            this.service.Delete(ev.ID);
            return RedirectToAction("Index", new { id = calendarID });
        }

        public ActionResult Details(int id)
        {
            var domain = this.service.Get(id);
            var model = DomainToModel.Map(domain);
            return View(model);
        }
    }
}