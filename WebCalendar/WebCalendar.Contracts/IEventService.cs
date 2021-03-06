﻿using System.Collections.Generic;
using WebCalendar.Domain.Aggregate.Event;

namespace WebCalendar.Contracts
{
    public interface IEventService
    {
        void Create(Event ev);
        void AddEvents(List<Event> evs);
        List<Event> GetEventsFromCalendar(int id);
        Event Get(int id);
        Event Get(int? id);
        List<Event> GetEvents { get; }
        void Update(Event ev);
        void Delete(int id);
    }
}
