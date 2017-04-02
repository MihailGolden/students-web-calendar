using System.Linq;
using System.Web.Mvc;
using WebCalendar.Domain.Aggregate.Calendar;
using WebCalendar.Domain.Aggregate.Event;
using WebCalendar.Mappers;
using WebCalendar.Models;

namespace WebCalendar.Controllers
{
    public class EventController : Controller
    {
        IEventRepository eventRepository;
        ICalendarRepository calendarRepository;

        public EventController(IEventRepository eventRepository, ICalendarRepository calendarRepository)
        {
            this.calendarRepository = calendarRepository;
            this.eventRepository = eventRepository;
        }

        public void InitDropDownList(EventViewModel model)
        {
            model.Calendars = from calendar in this.calendarRepository.Entities
                              select new SelectListItem { Text = calendar.Title, Value = calendar.ID.ToString() };
        }

        // GET: Event
        public ActionResult Index(int id)
        {
            var calendars = this.calendarRepository.Entities.FirstOrDefault(c => c.ID == id);
            var events = this.eventRepository.GetEvents(calendars);
            return View(events);
        }

        public ActionResult Create()
        {
            EventViewModel model = new EventViewModel();
            InitDropDownList(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(EventViewModel ev)
        {
            if (ev != null)
            {
                var domain = DomainToModel.Map(ev);
                this.eventRepository.Add(domain);
            }
            return RedirectToAction("Index", new { id = ev.CalendarID });
        }

        public ActionResult Update(int id)
        {
            var ev = this.eventRepository.Entities.FirstOrDefault(e => e.ID == id);
            var model = DomainToModel.Map(ev);
            InitDropDownList(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(EventViewModel ev)
        {
            int calendarID = ev.CalendarID;
            var domain = DomainToModel.Map(ev);
            this.eventRepository.Update(domain);
            return RedirectToAction("Index", new { id = calendarID });
        }

        public ActionResult Delete(int? id)
        {
            var ev = this.eventRepository.Entities.FirstOrDefault(e => e.ID == id);
            var model = DomainToModel.Map(ev);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(EventViewModel ev)
        {
            int calendarID = ev.CalendarID;
            this.eventRepository.Delete(ev.ID);
            return RedirectToAction("Index", new { id = calendarID });
        }
    }
}