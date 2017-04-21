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
    public class CalendarController : Controller
    {
        private ICalendarService service;
        private IUserService userService;

        public CalendarController(ICalendarService service, IUserService userService)
        {
            this.service = service;
            this.userService = userService;
        }
        // GET: Calendar

        public ActionResult Index()
        {
            return View();
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
                this.service.Delete(id);
            }
            return Json(cal);
        }

        public ActionResult CreateModal(DateTime begin, DateTime end)
        {
            ViewBag.BeginEvennt = begin;
            ViewBag.EndEvent = end;

            return View();
        }
    }
}