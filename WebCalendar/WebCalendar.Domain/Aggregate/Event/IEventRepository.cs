using System.Collections.Generic;
using WebCalendar.Domain.Abstract;

namespace WebCalendar.Domain.Aggregate.Event
{
    public interface IEventRepository : IRepository<Event>
    {
        List<Event> GetEvents(Calendar.Calendar cal);
    }
}
