using Ninject.Modules;
using WebCalendar.Contracts;

namespace WebCalendar.Services.Infrastructure
{
    public class NinjectSericeModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICalendarService>().To<CalendarService>();
        }
    }
}
