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
            var calendars = this.repository.Entities.ToList();
            return View(calendars);
        }
        public ActionResult GridHtml(string fileName)
        {
            string pathToFile = string.Format("~/Views/Calendar/{0}", fileName);

            var result = new FilePathResult(pathToFile, "text/html");
            return result;
        }
    }
}