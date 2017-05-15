using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebCalendar.Contracts;
using WebCalendar.Mappers;
using WebCalendar.Models;

namespace WebCalendar.Controllers
{
    [Authorize]
    //[ValidateAntiForgeryToken]
    public class CalendarController : Controller
    {
        private ICalendarService service;
        private IUserService userService;
        private IEventService eventService;
        private INotificationService notifyService;

        public CalendarController(ICalendarService service, IUserService userService, IEventService eventService, INotificationService notifyService)
        {
            this.service = service;
            this.userService = userService;
            this.eventService = eventService;
            this.notifyService = notifyService;
        }
        // GET: Calendar

        public ActionResult Index()
        {
            //int calendarId;
            //var calendars = this.service.GetUserCalendars();
            //List<CalendarViewModel> list = DomainToModel.Map(calendars);
            //if (list.Count == 0)
            //{
            //    CalendarViewModel newCalendar = new CalendarViewModel
            //    {
            //        Title = "Calendar",
            //        Description = "Default calendar",
            //        UserID = this.userService.GetUserID(),
            //    };
            //    var domain = DomainToModel.Map(newCalendar);
            //    this.service.Create(domain);
            //    //return View();

            //    calendarId = GetCalendarId("Calendar");
            //    return RedirectToAction("Open", new { calendarId = calendarId });
            //}
            return View();
        }


        public int GetCalendarId(string name)
        {
            int calendarId;
            var calendar = this.service.GetUserCalendars().FirstOrDefault(c => c.Title == name);
            calendarId = calendar.ID;

            return calendarId;
        }

        public ActionResult GridHtml(string fileName)
        {
            return View(fileName);
        }

        public JsonResult List()
        {
            var calendars = this.service.GetUserCalendars();
            List<CalendarViewModel> list = DomainToModel.Map(calendars);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Open(int calendarID)
        {
            var calendar = this.service.GetUserCalendars().FirstOrDefault(c => c.ID == calendarID);
            var model = DomainToModel.Map(calendar);
            return View(model);
        }

        public JsonResult Create(CalendarViewModel cal)
        {
            if (cal != null)
            {
                cal.UserID = this.userService.GetUserID();
                var domain = DomainToModel.Map(cal);
                this.service.Create(domain);
            }
            return Json(cal);
        }

        [HttpPost]
        public JsonResult Update(CalendarViewModel cal)
        {
            if (cal != null)
            {
                cal.UserID = this.userService.GetUserID();
                var domain = DomainToModel.Map(cal);
                this.service.Update(domain);
            }
            return Json(cal);
        }

        public JsonResult GetbyID(int id)
        {
            var cal = this.service.GetUserCalendars().FirstOrDefault(c => c.ID == id);
            var model = DomainToModel.Map(cal);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var cal = this.service.GetUserCalendars().FirstOrDefault(c => c.ID == id);
            if (cal != null)
            {
                var events = this.eventService.GetEventsFromCalendar(id);
                foreach (var item in events)
                {
                    var notify = this.notifyService.GetNotificationFromEvent(item.ID);
                    if (notify != null)
                    {
                        this.notifyService.Delete(notify.ID);
                    }
                }
                this.service.Delete(id);
            }
            return Json(cal);
        }
    }
}