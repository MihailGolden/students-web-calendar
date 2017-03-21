using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCalendar.Domain.Abstract;

namespace WebCalendar.Domain.Aggregate.Calendar
{
   public interface ICalendarRepository:IRepository<Calendar>
    {
    }
}
