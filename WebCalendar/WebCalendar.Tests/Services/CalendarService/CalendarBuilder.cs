using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using WebCalendar.Domain.Aggregate.Calendar;

namespace WebCalendar.Tests.Services.CalendarService
{
    [ExcludeFromCodeCoverage]
    public class CalendarBuilder
    {
        private Calendar calendar;
        private List<Calendar> cals;
        public CalendarBuilder()
        {
            this.calendar = new Calendar()
            {
                ID = 1,
                Title = "MVC",
                Description = "Learn Asp.Net MVC"
            };
            this.cals = new List<Calendar>() { this.calendar };
        }
        public Calendar WithUserID(string userID)
        {
            this.calendar.UserID = userID;
            return this.calendar;
        }
        public Calendar Build()
        {
            return this.calendar;
        }
        public List<Calendar> BuildList()
        {
            return this.cals;
        }
    }
}
