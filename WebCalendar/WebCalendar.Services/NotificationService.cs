using System;
using System.Collections.Generic;
using System.Linq;
using WebCalendar.Contracts;
using WebCalendar.Domain.Aggregate.Notification;

namespace WebCalendar.Services
{
    public class NotificationService : INotificationService
    {
        INotificationRepository notificationRepository;
        public NotificationService(INotificationRepository notificationRepository)
        {
            this.notificationRepository = notificationRepository;
        }
        public List<Notification> GetNotifications
        {
            get
            {
                return this.notificationRepository.Entities.ToList();
            }
        }

        public void AddNotifications(List<Notification> notifications)
        {
            if (notifications != null)
            {
                foreach (var item in notifications)
                {
                    this.notificationRepository.Add(item);
                }
            }
            else
            {
                throw new ArgumentNullException("Null notifications!");
            }
        }

        public void Create(Notification notify)
        {
            if (notify == null)
            {
                throw new ArgumentNullException("Null notification!");
            }
            if (notify != null)
            {
                this.notificationRepository.Add(notify);
            }
        }

        public void Delete(int id)
        {
            var notification = this.notificationRepository.Entities.FirstOrDefault(n => n.ID == id);
            if (notification == null)
            {
                throw new ArgumentNullException("Null notification!");
            }
            this.notificationRepository.Delete(id);
        }

        public Notification Get(int id)
        {
            var notify = this.notificationRepository.Entities.FirstOrDefault(n => n.ID == id);
            return notify;
        }

        public Notification GetNotificationFromEvent(int eventID)
        {
            var notify = this.notificationRepository.Entities.FirstOrDefault(n => n.EventID == eventID);
            return notify;
        }

        public void Update(Notification notify)
        {
            try
            {
                this.notificationRepository.Update(notify);
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
