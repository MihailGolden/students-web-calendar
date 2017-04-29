using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using WebCalendar.Domain.Aggregate.Notification;

namespace WebCalendar.Tests.Services.NotificationService
{
    [ExcludeFromCodeCoverage]
    public class NotificationBuilder
    {
        private Notification notify;
        private List<Notification> notifications;
        public NotificationBuilder()
        {
            this.notify = new Notification()
            {
                ID = 1,
                Type = "alert",
                NotificateBeforeDay = 1,
                EventID = 1

            };
            this.notifications = new List<Notification>() { notify };
        }
        public Notification Build()
        {
            return this.notify;
        }
        public List<Notification> BuildList()
        {
            return this.notifications;
        }
    }
}
