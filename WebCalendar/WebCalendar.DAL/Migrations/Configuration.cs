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
            CalendarEntity cal = new CalendarEntity { ID = 1, Title = "My First Calendar" };
            EventEntity ev = new EventEntity { ID = 1, Description = "First Event", StartDate = new DateTime(2017, 02, 03), EndDate = new DateTime(2017, 03, 03), CalendarID = 1 };
            cal.Events.Add(ev);
            context.Calendars.AddOrUpdate(cal);
            context.Events.AddOrUpdate(ev);
        }
    }
}
