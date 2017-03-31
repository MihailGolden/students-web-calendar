using System;

namespace WebCalendar.Domain.Aggregate.Event
{
    public class Event
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int CalendarID { get; set; }
    }
}
