using System;
using WebCalendar.DAL.Entities;
using WebCalendar.Domain.Aggregate.Calendar;
using WebCalendar.Domain.Aggregate.Event;
using WebCalendar.Domain.Aggregate.Notification;
using WebCalendar.Domain.Aggregate.Occurrence;

namespace WebCalendar.DAL.Mappers
{
    public static class DomainToDal
    {
        public static void Map(CalendarEntity to, Calendar from)
        {
            to.CalendarID = from.ID;
            to.Title = from.Title;
            to.Date = from.Date;
            to.Description = from.Description;
        }

        public static Calendar Map(CalendarEntity to)
        {
            return new Calendar { ID = to.CalendarID, Title = to.Title, Date = to.Date, Description = to.Description };
        }

        public static void Map(EventEntity to, Event from)
        {
            to.EventID = from.ID;
            to.Title = from.Title;
            to.Description = from.Description;
            to.BeginTime = from.BeginTime;
            to.EndTime = from.EndTime;
            to.CalendarID = from.CalendarID;
        }

        internal static void Map(OccurrenceEntity entity, Occurrence occur)
        {
            throw new NotImplementedException();
        }

        internal static void Map(CalendarEntity entity, Occurrence occur)
        {
            throw new NotImplementedException();
        }

        internal static Occurrence Map(OccurrenceEntity item)
        {
            throw new NotImplementedException();
        }

        internal static void Map(NotificationEntity entity, Notification notify)
        {
            throw new NotImplementedException();
        }

        public static Event Map(EventEntity to)
        {
            return new Event { ID = to.EventID, Description = to.Description, CalendarID = to.CalendarID, BeginTime = to.BeginTime, EndTime = to.EndTime, Title = to.Title };
        }

        internal static Notification Map(NotificationEntity item)
        {
            throw new NotImplementedException();
        }
    }
}
