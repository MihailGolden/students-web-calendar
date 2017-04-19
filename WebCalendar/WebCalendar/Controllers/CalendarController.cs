using System.Linq;
using System.Web.Mvc;
using WebCalendar.Contracts;
using WebCalendar.Domain.Aggregate.Calendar;

namespace WebCalendar.Controllers
{
    public class CalendarController : Controller
    {
        private ICalendarService service;

        public CalendarController(ICalendarService service)
        {
            this.service = service;
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
            var calendars = this.service.GetCalendars;
            return Json(calendars, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Open(int calendarID)
        {
            var calendar = this.service.GetCalendars.FirstOrDefault(c => c.ID == calendarID);
            return View(calendar);
        }

        public JsonResult Create(Calendar cal)
        {
            if (cal != null)
            {
                this.service.Create(cal);
            }
            return Json(cal);
        }

        [HttpPost]
        public JsonResult Update(Calendar cal)
        {
            if (cal != null)
            {
                this.service.Update(cal);
            }
            return Json(cal);
        }

        public JsonResult GetbyID(int id)
        {
            var cal = this.service.GetCalendars.FirstOrDefault(c => c.ID == id);
            return Json(cal, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var cal = this.service.GetCalendars.FirstOrDefault(c => c.ID == id);
            if (cal != null)
            {
                this.service.Delete(id);
            }
            return Json(cal);
        }
    }
}