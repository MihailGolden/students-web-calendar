using System.Linq;
using System.Web.Mvc;
using WebCalendar.Domain.Aggregate.Calendar;

namespace WebCalendar.Controllers
{
    public class CalendarController : Controller
    {
        private ICalendarRepository repository;

        public CalendarController(ICalendarRepository repository)
        {
            this.repository = repository;
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
            var calendars = this.repository.Entities.ToList();
            return Json(calendars, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Open(int calendarID)
        {
            var calendar = this.repository.Entities.FirstOrDefault(c => c.ID == calendarID);
            return View(calendar);
        }

        public JsonResult Create(Calendar cal)
        {
            if (cal != null)
            {
                this.repository.Add(cal);
            }
            return Json(cal);
        }

        [HttpPost]
        public JsonResult Update(Calendar cal)
        {
            if (cal != null)
            {
                this.repository.Update(cal);
            }
            return Json(cal);
        }

        public JsonResult GetbyID(int id)
        {
            var cal = this.repository.Entities.FirstOrDefault(c => c.ID == id);
            return Json(cal, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var cal = this.repository.Entities.FirstOrDefault(c => c.ID == id);
            if (cal != null)
            {
                this.repository.Delete(id);
            }
            return Json(cal);
        }
    }
}