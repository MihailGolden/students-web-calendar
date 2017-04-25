using System.Collections.Generic;
using WebCalendar.Domain.Aggregate.Calendar;
using WebCalendar.Domain.Aggregate.Event;
using WebCalendar.Domain.Aggregate.Notification;
using WebCalendar.Models;

namespace WebCalendar.Mappers
{
    public static class DomainToModel
    {
        internal static Event Map(EventViewModel ev)
        {
            return new Event() { ID = ev.ID, Title = ev.Title, BeginTime = ev.BeginTime, EndTime = ev.EndTime, Description = ev.Description, CalendarID = ev.CalendarID };
        }

        internal static EventViewModel Map(Event ev)
        {
            return new EventViewModel() { ID = ev.ID, Title = ev.Title, Description = ev.Description, BeginTime = ev.BeginTime, EndTime = ev.EndTime, CalendarID = ev.CalendarID};
        }

        internal static CalendarViewModel Map(Calendar cal)
        {
            return new CalendarViewModel() { ID = cal.ID, Title = cal.Title, Description = cal.Description, UserID = cal.UserID };
        }

        internal static List<CalendarViewModel> Map(List<Calendar> calendars)
        {
            List<CalendarViewModel> list = new List<CalendarViewModel>();
            if (calendars != null)
            {
                foreach (var item in calendars)
                {
                    list.Add(Map(item));
                }
            }
            return list;
        }

        internal static List<EventViewModel> Map(List<Event> events)
        {
            List<EventViewModel> list = new List<EventViewModel>();
            if (events != null)
            {
                foreach (var item in events)
                {
                    list.Add(Map(item));
                }
            }
            return list;
        }

        internal static Calendar Map(CalendarViewModel cal)
        {
            return new Calendar { ID = cal.ID, Description = cal.Description, Title = cal.Title, UserID = cal.UserID };
        }

        internal static Notification Map(NotificationViewModel notificationViewModel)
        {
            return new Notification { ID = notificationViewModel.ID, EventID = notificationViewModel.EventID, NotificateBeforeDay = notificationViewModel.NotificateBeforeDay, NotificationDefaultTime = notificationViewModel.NotificationDefaultTime, Type = notificationViewModel.Type };
        }
    }
}