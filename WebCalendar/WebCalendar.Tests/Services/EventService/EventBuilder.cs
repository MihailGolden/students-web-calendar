using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using WebCalendar.Domain.Aggregate.Event;

namespace WebCalendar.Tests.Services.EventService
{
    [ExcludeFromCodeCoverage]
    public class EventBuilder
    {
        private Event ev;
        private List<Event> events;

        public EventBuilder()
        {
            this.ev = new Event()
            {
                ID = 1,
                Title = "Go to Work",
                BeginTime = new DateTime(2017, 04, 27, 15, 30, 0),
                EndTime = new DateTime(2017, 04, 27, 17, 30, 0),
                Description = "working"
            };
            this.events = new List<Event>() { ev };
        }
        public Event Build()
        {
            return this.ev;
        }
        public List<Event> BuildList()
        {
            return this.events;
        }
    }
}
