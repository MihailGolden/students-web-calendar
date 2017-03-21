using WebCalendar.DAL.Entities;
using WebCalendar.Domain.Aggregate.Calendar;
using WebCalendar.Domain.Aggregate.Event;

namespace WebCalendar.DAL.Mappers
{
    public static class DomainToDal
    {
        public static void Map(CalendarEntity to, Calendar from)
        {
            to.ID = from.ID;
            to.Title = from.Title;
        }

        public static Calendar Map(CalendarEntity to)
        {
            return new Calendar { ID = to.ID, Title = to.Title };
        }

        public static void Map(EventEntity to, Event from)
        {
            to.ID = from.ID;
            to.Description = from.Description;
            to.StartDate = from.StartDate;
            to.EndDate = from.EndDate;
            to.CalendarID = from.CalendarID;
        }

        public static Event Map(EventEntity to)
        {
            return new Event { ID = to.ID, Description = to.Description, CalendarID = to.CalendarID, StartDate = to.StartDate, EndDate = to.EndDate };
        }
    }
}
