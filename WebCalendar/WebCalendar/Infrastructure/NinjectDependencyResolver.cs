using Ninject;
using System;
using System.Collections.Generic;
using WebCalendar.DAL.Concrete;
using WebCalendar.Domain.Aggregate.Calendar;
using WebCalendar.Domain.Aggregate.Event;

namespace WebCalendar.Infrastructure
{
    public class NinjectDependencyResolver : System.Web.Mvc.IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<ICalendarRepository>().To<CalendarRepository>();
            kernel.Bind<IEventRepository>().To<EventRepository>();
        }
    }
}