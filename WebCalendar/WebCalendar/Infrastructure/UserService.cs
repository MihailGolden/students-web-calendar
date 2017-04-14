using Microsoft.AspNet.Identity;
using System.Web;

namespace WebCalendar.Infrastructure
{
    public class UserService : IUserService
    {
        public int GetUserId()
        {
            return System.Convert.ToInt32(HttpContext.Current.User.Identity.GetUserId());
        }
    }
}