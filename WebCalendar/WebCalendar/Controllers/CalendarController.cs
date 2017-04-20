using System.Linq;
using System.Web.Mvc;
using WebCalendar.Contracts;
using WebCalendar.Domain.Aggregate.Calendar;

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
            return Json(calendars, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Open(int calendarID)
        {
            var calendar = this.service.GetUserCalendars().FirstOrDefault(c => c.ID == calendarID);
            return View(calendar);
        }

        public JsonResult Create(Calendar cal)
        {
            if (cal != null)
            {
                cal.UserID = this.userService.GetUserID();
                this.service.Create(cal);
            }
            return Json(cal);
        }

        [HttpPost]
        public JsonResult Update(Calendar cal)
        {
            if (cal != null)
            {
                cal.UserID = this.userService.GetUserID();
                this.service.Update(cal);
            }
            return Json(cal);
        }

        public JsonResult GetbyID(int id)
        {
            var cal = this.service.GetUserCalendars().FirstOrDefault(c => c.ID == id);
            return Json(cal, JsonRequestBehavior.AllowGet);
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
    }
}