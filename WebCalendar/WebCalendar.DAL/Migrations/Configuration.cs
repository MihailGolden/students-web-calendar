namespace WebCalendar.DAL.Migrations
{
    using Entities;
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<WebCalendar.DAL.Context.WebCalendarContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WebCalendar.DAL.Context.WebCalendarContext context)
        {
            CalendarEntity cal = new CalendarEntity { CalendarID = 1, Title = "My First Calendar", Date = new DateTime(2017, 03, 31) };
            EventEntity ev = new EventEntity { EventID = 1, Title = "First Event", Description = "To-Do something", CalendarID = 1, BeginTime = new DateTime(2017, 03, 27, 12, 10, 0) };
            cal.Events.Add(ev);
            context.Calendars.AddOrUpdate(cal);
            context.Events.AddOrUpdate(ev);
        }
    }
}
