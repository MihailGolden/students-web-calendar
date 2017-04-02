using WebCalendar.DAL.Entities;
using WebCalendar.Domain.Aggregate.Calendar;
using WebCalendar.Domain.Aggregate.Event;

namespace WebCalendar.DAL.Mappers
{
    public static class DomainToDal
    {
        public static void Map(CalendarEntity to, Calendar from)
        {
            to.CalendarID = from.ID;
            to.Title = from.Title;
            to.Date = from.Date;
            to.Description = from.Description;
        }

        public static Calendar Map(CalendarEntity to)
        {
            return new Calendar { ID = to.CalendarID, Title = to.Title, Date = to.Date, Description = to.Description };
        }

        public static void Map(EventEntity to, Event from)
        {
            to.EventID = from.ID;
            to.Title = from.Title;
            to.Description = from.Description;
            to.BeginTime = from.BeginTime;
            to.EndTime = from.EndTime;
            to.CalendarID = from.CalendarID;
        }

        public static Event Map(EventEntity to)
        {
            return new Event { ID = to.EventID, Description = to.Description, CalendarID = to.CalendarID, BeginTime = to.BeginTime, EndTime = to.EndTime, Title = to.Title };
        }
    }
}
