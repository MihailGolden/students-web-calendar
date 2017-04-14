using Ninject.Activation;
using Ninject.Infrastructure;
using Ninject.Modules;
using System;
using WebCalendar.DAL.Concrete;
using WebCalendar.DAL.Context;
using WebCalendar.DAL.Helpers;
using WebCalendar.DAL.Repositories;
using WebCalendar.Domain.Aggregate.Calendar;
using WebCalendar.Domain.Aggregate.Event;
using WebCalendar.Domain.Aggregate.Notification;
using WebCalendar.Domain.Aggregate.Occurrence;

namespace WebCalendar.DAL.Infrastructure
{
    public class NinjectDataAccessModule : NinjectModule
    {
        private readonly Func<IContext, object> scope;


        public NinjectDataAccessModule(Func<IContext, object> scope)
        {
            this.scope = scope;
        }

        public override void Load()
        {
            var configs = new IHaveBindingConfiguration[]
                              {
                                  Bind<IUnitOfWork>().To<UnitOfWork>(),
                                  Bind<IEventRepository>().To<EventRepository>(),
                                  Bind<ICalendarRepository>().To<CalendarRepository>(),
                                  Bind<INotificationRepository>().To<NotificationRepository>(),
                                  Bind<IOccurrenceRepository>().To<OccurenceRepository>()
                              };

            configs.InScope(scope);
        }
    }
}
