using System;
using System.Collections.Generic;
using System.Linq;
using WebCalendar.Contracts;
using WebCalendar.Domain.Aggregate.Event;

namespace WebCalendar.Services
{
    public class EventService : IEventService
    {
        private IEventRepository repository;
        public EventService(IEventRepository repository)
        {
            this.repository = repository;
        }
        public List<Event> GetEvents
        {
            get
            {
                return this.repository.Entities.ToList();
            }
        }

        public void AddEvents(List<Event> evs)
        {
            if (evs != null)
            {
                foreach (var item in evs)
                {
                    this.repository.Add(item);
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
                this.repository.Add(ev);
            }
        }

        public void Delete(int id)
        {
            var ev = this.repository.Entities.FirstOrDefault(e => e.ID == id);
            if (ev == null)
            {
                throw new ArgumentNullException("Null calendar!");
            }
            this.repository.Delete(id);
        }

        public Event Get(int id)
        {
            var ev = this.repository.Entities.FirstOrDefault(e => e.ID == id);
            return ev;
        }

        public void Update(Event ev)
        {
            try
            {
                this.repository.Update(ev);
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
