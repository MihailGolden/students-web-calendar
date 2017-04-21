using System;
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

        public NotificationRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null) throw new ArgumentNullException("Null context!");
            this.unitOfWork = (UnitOfWork)unitOfWork;
            this.dal = this.unitOfWork.Context.Notifications;
        }

        public void Add(Notification notify)
        {
            if (notify == null) throw new ArgumentNullException("entity");
            var entity = new NotificationEntity();
            DomainToDal.Map(entity, notify);
            this.dal.Add(entity);
            this.unitOfWork.Commit();
            this.unitOfWork.Dispose();
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
            if (entity == null) throw new ArgumentNullException("entity");
            this.dal.Attach(entity);
            this.dal.Remove(entity);
            this.unitOfWork.Commit();
            this.unitOfWork.Dispose();
        }

        public void Update(Notification entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            var newEntity = new NotificationEntity();
            DomainToDal.Map(newEntity, entity);
            if (entity.ID == Constants.NEW_DATABASE_ID_VALUE)
            {
                dal.Add(newEntity);
            }
            else
            {
                NotificationEntity notify = dal.Find(entity.ID);
                notify.Type = newEntity.Type;
                notify.EventID = newEntity.EventID;
                notify.NotificateBeforeDay = newEntity.NotificateBeforeDay;
                notify.NotificationDefaultTime = notify.NotificationDefaultTime;
            }
            this.unitOfWork.Commit();
            this.unitOfWork.Dispose();
        }
    }
}
