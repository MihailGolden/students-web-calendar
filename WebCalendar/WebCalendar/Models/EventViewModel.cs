using System;
using System.Linq;

namespace WebCalendar.Models
{
    public class EventViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int CalendarID { get; set; }
        public IQueryable Calendars { get; set; }
    }
}