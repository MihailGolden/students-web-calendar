using System;
using System.Collections.Generic;
using System.Linq;
using WebCalendar.Contracts;
using WebCalendar.Domain.Aggregate.Calendar;
using WebCalendar.Domain.Aggregate.Event;

namespace WebCalendar.Services
{
    public class EventService : IEventService
    {
        private IEventRepository eventRepository;
        private ICalendarRepository calendarRepository;
        public EventService(IEventRepository eventRepository, ICalendarRepository calendarRepository)
        {
            this.eventRepository = eventRepository;
            this.calendarRepository = calendarRepository;
        }
        public List<Event> GetEvents
        {
            get
            {
                return this.eventRepository.Entities.ToList();
            }
        }

        public void AddEvents(List<Event> evs)
        {
            if (evs != null)
            {
                foreach (var item in evs)
                {
                    this.eventRepository.Add(item);
                }
            }
            else
            {
                throw new ArgumentNullException("Null events!");
            }
        }

        public void Create(Event ev)
        {
            if (ev == null)
            {
                throw new ArgumentNullException("Null event!");
            }
            if (ev != null)
            {
                this.eventRepository.Add(ev);
            }
        }

        public void Delete(int id)
        {
            var ev = this.eventRepository.Entities.FirstOrDefault(e => e.ID == id);
            if (ev == null)
            {
                throw new ArgumentNullException("Null calendar!");
            }
            this.eventRepository.Delete(id);
        }

        public Event Get(int id)
        {
            var ev = this.eventRepository.Entities.FirstOrDefault(e => e.ID == id);
            return ev;
        }

        public Event Get(int? id)
        {
            var ev = this.eventRepository.Entities.FirstOrDefault(e => e.ID == id);
            return ev;
        }

        public List<Event> GetEventsFromCalendar(int id)
        {
            var calendar = this.calendarRepository.Entities.FirstOrDefault(c => c.ID == id);
            var events = this.eventRepository.GetEvents(calendar);
            return events;
        }

        public void Update(Event ev)
        {
            try
            {
                this.eventRepository.Update(ev);
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
