using WebCalendar.Domain.Aggregate.Event;
using WebCalendar.Models;

namespace WebCalendar.Mappers
{
    public static class DomainToModel
    {
        internal static Event Map(EventViewModel ev)
        {
            return new Event() { ID = ev.ID, Title = ev.Title, BeginTime = ev.BeginTime, EndTime = ev.EndTime, Description = ev.Description, CalendarID = ev.CalendarID };
        }

        internal static EventViewModel Map(Event ev)
        {
            return new EventViewModel() { ID = ev.ID, Title = ev.Title, Description = ev.Description, BeginTime = ev.BeginTime, EndTime = ev.EndTime, CalendarID = ev.CalendarID };
        }
    }
}