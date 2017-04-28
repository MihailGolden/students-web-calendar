using Ninject.Modules;
using WebCalendar.Contracts;

namespace WebCalendar.Services.Infrastructure
{
    public class NinjectSericeModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICalendarService>().To<CalendarService>();
            Bind<IEventService>().To<EventService>();
            Bind<INotificationService>().To<NotificationService>();
            Bind<IOccurrenceService>().To<OccurrenceService>();
        }
    }
}
