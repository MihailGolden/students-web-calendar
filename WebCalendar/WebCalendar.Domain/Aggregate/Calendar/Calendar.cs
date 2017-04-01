using System;

namespace WebCalendar.Domain.Aggregate.Calendar
{
    public class Calendar
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
    }
}
