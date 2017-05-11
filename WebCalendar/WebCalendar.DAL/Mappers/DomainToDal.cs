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
            to.UserID = from.UserID;
            to.Description = from.Description;
        }

        public static Calendar Map(CalendarEntity to)
        {
            return new Calendar { ID = to.CalendarID, Title = to.Title, UserID = to.UserID, Description = to.Description };
        }

        public static void Map(EventEntity to, Event from)
        {
            to.EventID = from.ID;
            to.Title = from.Title;
            to.Description = from.Description;
            to.BeginTime = from.BeginTime;
            to.EndTime = from.EndTime;
            to.EventColor = from.EventColor;
            to.CalendarID = from.CalendarID;
            to.OccurrenceID = from.OccurrenceID;
        }

        internal static void Map(OccurrenceEntity entity, Occurrence occur)
        {
            entity.OccurenceID = occur.ID;
            entity.Count = occur.Count;
        }

        internal static Occurrence Map(OccurrenceEntity item)
        {
            return new Occurrence { ID = item.OccurenceID, Count = item.Count };
        }

        internal static void Map(NotificationEntity entity, Notification notify)
        {
            entity.NotificationID = notify.ID;
            entity.Type = notify.Type;
            entity.NotificationDefaultTime = notify.NotificationDefaultTime;
            entity.NotificateBeforeDay = notify.NotificateBeforeDay;
            entity.EventID = notify.EventID;
        }

        public static Event Map(EventEntity to)
        {
            return new Event { ID = to.EventID, Description = to.Description, CalendarID = to.CalendarID, BeginTime = to.BeginTime, EndTime = to.EndTime, EventColor = to.EventColor, Title = to.Title, OccurrenceID = to.OccurrenceID };
        }

        internal static Notification Map(NotificationEntity item)
        {
            return new Notification { ID = item.NotificationID, Type = item.Type, NotificateBeforeDay = item.NotificateBeforeDay, NotificationDefaultTime = item.NotificationDefaultTime, EventID = item.EventID };
        }
    }
}
