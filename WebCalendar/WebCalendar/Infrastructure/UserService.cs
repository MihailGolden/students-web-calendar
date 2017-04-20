using Microsoft.AspNet.Identity;
using System.Web;
using WebCalendar.Contracts;

namespace WebCalendar.Infrastructure
{
    public class UserService : IUserService
    {
        public string GetUserID()
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }
        public string GetUserName()
        {
            return HttpContext.Current.User.Identity.GetUserName();
        }
    }
}