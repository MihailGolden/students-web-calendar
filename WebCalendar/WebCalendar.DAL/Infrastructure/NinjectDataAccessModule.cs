using Ninject.Modules;
using WebCalendar.DAL.Concrete;
using WebCalendar.DAL.Context;
using WebCalendar.DAL.Repositories;
using WebCalendar.Domain.Aggregate.Calendar;
using WebCalendar.Domain.Aggregate.Event;
using WebCalendar.Domain.Aggregate.Notification;
using WebCalendar.Domain.Aggregate.Occurrence;

namespace WebCalendar.DAL.Infrastructure
{
    public class NinjectDataAccessModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<IEventRepository>().To<EventRepository>();
            Bind<ICalendarRepository>().To<CalendarRepository>();
            Bind<INotificationRepository>().To<NotificationRepository>();
            Bind<IOccurrenceRepository>().To<OccurenceRepository>();
        }
    }
}
