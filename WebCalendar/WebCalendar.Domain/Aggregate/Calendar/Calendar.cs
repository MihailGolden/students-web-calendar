using System;
using WebCalendar.Domain.Properties;

namespace WebCalendar.Domain.Aggregate.Calendar
{
    public class Calendar
    {
        private string title;
        private string description;
        public int ID { get; set; }
        public string Title
        {
            get { return this.title; }
            set
            {
                if (CalendarValidation.ValidateTitle(value))
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
                if (CalendarValidation.ValidateDescription(value))
                {
                    throw new ArgumentException(Resources.ValidationDescription, "Description");
                }
                this.description = value;
            }
        }
        public DateTime? Date { get; set; }
    }
}
