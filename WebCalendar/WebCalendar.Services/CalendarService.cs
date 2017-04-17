using System;
using System.Collections.Generic;
using System.Linq;
using WebCalendar.Contracts;
using WebCalendar.Domain.Aggregate.Calendar;

namespace WebCalendar.Services
{
    public class CalendarService : ICalendarService
    {
        private ICalendarRepository repository;
        public CalendarService(ICalendarRepository repository)
        {
            this.repository = repository;
        }
        public List<Calendar> GetCalendars
        {
            get
            {
                return this.repository.Entities.ToList();
            }
        }

        public void AddCalendars(List<Calendar> cals)
        {
            if (cals != null)
            {
                foreach (var item in cals)
                {
                    this.repository.Add(item);
                }
            }
            else
            {
                throw new ArgumentNullException("Null calendars!");
            }
        }

        public void Create(Calendar cal)
        {
            if (cal == null)
            {
                throw new ArgumentNullException("Null calendar!");
            }
            if (cal != null)
            {
                this.repository.Add(cal);
            }
        }

        public void Delete(int id)
        {
            var calendar = this.repository.Entities.FirstOrDefault(c => c.ID == id);
            if (calendar == null)
            {
                throw new ArgumentNullException("Null calendar!");
            }
            this.repository.Delete(id);
        }

        public Calendar Get(int id)
        {
            var calendar = this.repository.Entities.FirstOrDefault(c => c.ID == id);
            return calendar;
        }

        public void Update(Calendar cal)
        {
            try
            {
                this.repository.Update(cal);
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
