using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebCalendar.Startup))]
namespace WebCalendar
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
