using System.Collections.Generic;
using WebCalendar.Domain.Aggregate.Notification;

namespace WebCalendar.Contracts
{
    public interface INotificationService
    {
        void Create(Notification notify);
        void AddNotifications(List<Notification> notifications);
        Notification Get(int id);
        List<Notification> GetNotifications { get; }
        Notification GetNotificationFromEvent(int eventID);
        void Update(Notification notify);
        void Delete(int id);
    }
}
