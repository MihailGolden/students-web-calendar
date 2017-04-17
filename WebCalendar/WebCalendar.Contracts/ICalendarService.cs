using System.Collections.Generic;
using WebCalendar.Domain.Aggregate.Calendar;

namespace WebCalendar.Contracts
{
    public interface ICalendarService
    {
        void Create(Calendar cal);
        void AddCalendars(List<Calendar> cals);
        Calendar Get(int id);
        List<Calendar> GetCalendars { get; }
        void Update(Calendar cal);
        void Delete(int id);
    }
}
