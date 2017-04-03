using System;
using WebCalendar.Domain.Properties;

namespace WebCalendar.Domain.Aggregate.Event
{
    public class Event
    {
        private string title;
        private string description;

        public int ID { get; set; }
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                if (EventValidation.ValidateTitle(value))
                {
                    throw new ArgumentException(Resources.ValidationTitle, "Title");
                }
                this.title = value;
            }
        }
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                if (EventValidation.ValidateDescription(value))
                {
                    throw new ArgumentException(Resources.ValidationDescription, "Description");
                }
                this.description = value;
            }
        }
        public DateTime BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int CalendarID { get; set; }
    }
}
