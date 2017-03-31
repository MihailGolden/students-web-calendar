﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WebCalendar.DAL.Context;
using WebCalendar.DAL.Entities;
using WebCalendar.DAL.Mappers;
using WebCalendar.Domain.Aggregate.Event;

namespace WebCalendar.DAL.Concrete
{
    public class EventRepository : IEventRepository
    {
        private readonly UnitOfWork unitOfWork;
        private readonly DbSet<EventEntity> dal;

        public EventRepository()
        {
            this.unitOfWork = new UnitOfWork();
            this.dal = unitOfWork.Context.Events;
        }

        public EventRepository(IUnitOfWork unitOfWork)
        {

        }

        public void Add(Event ev)
        {
            var entity = new EventEntity();
            DomainToDal.Map(entity, ev);
            this.dal.Add(entity);
            this.unitOfWork.Commit();
            ev.ID = entity.EventID;
        }

        public IQueryable<Event> Entities
        {
            get
            {
                var list = new List<Event>();
                var ev = unitOfWork.Context.Events.ToList();
                foreach (var item in ev)
                {
                    list.Add(DomainToDal.Map(item));
                }
                return list.AsQueryable();
            }
        }

        public void Delete(int id)
        {
            EventEntity entity = this.dal.Find(id);
            if (entity != null)
            {
                this.dal.Attach(entity);
                this.dal.Remove(entity);
                this.unitOfWork.Commit();
            }
        }

        public void Update(Event entity)
        {
            var newEntity = new EventEntity();
            DomainToDal.Map(newEntity, entity);
            if (entity.ID == 0)
            {
                dal.Add(newEntity);
            }
            else
            {
                EventEntity ev = dal.Find(entity.ID);
                if (ev != null)
                {
                    ev.Description = entity.Description;
                    ev.BeginTime = entity.BeginTime;
                    ev.EndTime = entity.EndTime;
                    ev.CalendarID = entity.CalendarID;
                }
            }
            this.unitOfWork.Commit();
        }
    }
}
