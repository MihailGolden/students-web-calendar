namespace WebCalendar.DAL.Migrations
{
    using Domain;
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
            CalendarEntity cal = new CalendarEntity { CalendarID = 1, Title = "MVC", /*Date = new DateTime(2017, 03, 15), */Description = "Learn Asp.Net MVC" };

            EventEntity ev = new EventEntity { EventID = 1, Title = "To-Do List", Description = "Create a Asp.Net Mvc application with database", CalendarID = 1, BeginTime = new DateTime(2017, 03, 16), EndTime = new DateTime(2017, 03, 20), EventColor = (int)Color.yellow, EveryMonth = true };
            EventEntity ev1 = new EventEntity { EventID = 2, Title = "JS", Description = "Write functions", CalendarID = 1, BeginTime = new DateTime(2017, 03, 16), EndTime = new DateTime(2017, 03, 20), EventColor = (int)Color.green, EveryMonth = true };
            EventEntity ev2 = new EventEntity { EventID = 3, Title = "Chat", Description = "Create a simple chat", CalendarID = 1, BeginTime = new DateTime(2017, 04, 16), EndTime = new DateTime(2017, 04, 25), EventColor = (int)Color.red, EveryMonth = true };
            EventEntity ev3 = new EventEntity { EventID = 4, Title = "User Authorization", Description = "Registration and Login", CalendarID = 1, BeginTime = new DateTime(2017, 01, 05), EndTime = new DateTime(2017, 05, 8), EventColor = (int)Color.blue, EveryMonth = true };

            cal.Events.Add(ev);
            cal.Events.Add(ev1);
            cal.Events.Add(ev2);
            cal.Events.Add(ev3);

            context.Calendars.AddOrUpdate(cal);
            context.Events.AddOrUpdate(ev, ev1, ev2, ev3);
        }
    }
}
