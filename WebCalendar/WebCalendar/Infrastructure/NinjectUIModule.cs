using Ninject.Modules;
using WebCalendar.Contracts;

namespace WebCalendar.Infrastructure
{
    public class NinjectUIModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserService>().To<UserService>();
        }
    }
}