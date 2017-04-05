using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WebCalendar.DAL.Context;
using WebCalendar.DAL.Entities;
using WebCalendar.DAL.Mappers;
using WebCalendar.Domain.Aggregate.Notification;

namespace WebCalendar.DAL.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly UnitOfWork unitOfWork;
        private readonly DbSet<NotificationEntity> dal;

        public NotificationRepository()
        {
            this.unitOfWork = new UnitOfWork();
            this.dal = unitOfWork.Context.Notifications;
        }

        public NotificationRepository(IUnitOfWork unitOfWork)
        {

        }

        public void Add(Notification notify)
        {
            var entity = new NotificationEntity();
            DomainToDal.Map(entity, notify);
            this.dal.Add(entity);
            this.unitOfWork.Commit();
            notify.ID = entity.NotificationID;
        }

        public IQueryable<Notification> Entities
        {
            get
            {
                var list = new List<Notification>();
                var notify = unitOfWork.Context.Notifications.ToList();
                foreach (var item in notify)
                {
                    list.Add(DomainToDal.Map(item));
                }
                return list.AsQueryable();
            }
        }

        public void Delete(int id)
        {
            NotificationEntity entity = this.dal.Find(id);
            if (entity != null)
            {
                this.dal.Attach(entity);
                this.dal.Remove(entity);
                this.unitOfWork.Commit();
            }
        }

        public void Update(Notification entity)
        {
            var newEntity = new NotificationEntity();
            DomainToDal.Map(newEntity, entity);
            if (entity.ID == 0)
            {
                dal.Add(newEntity);
            }
            else
            {
                NotificationEntity notify = dal.Find(entity.ID);
                if (notify != null)
                {
                    notify.Type = newEntity.Type;
                    notify.EventID = newEntity.EventID;
                    notify.NotificateBeforeDay = newEntity.NotificateBeforeDay;
                    notify.NotificationDefaultTime = notify.NotificationDefaultTime;
                }
            }
            this.unitOfWork.Commit();
        }
    }
}
