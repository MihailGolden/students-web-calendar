﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WebCalendar.DAL.Context;
using WebCalendar.DAL.Entities;
using WebCalendar.DAL.Mappers;
using WebCalendar.Domain.Aggregate.Calendar;
using WebCalendar.Domain.Aggregate.Event;

namespace WebCalendar.DAL.Concrete
{
    public class EventRepository : IEventRepository
    {
        private readonly UnitOfWork unitOfWork;
        private readonly DbSet<EventEntity> dal;

        public EventRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = (UnitOfWork)unitOfWork;
            this.dal = this.unitOfWork.Context.Events;
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
                    ev.Title = entity.Title;
                    ev.OccurrenceID = entity.OccurrenceID;
                    ev.EventColor = entity.EventColor;
                    ev.EveryDay = entity.EveryDay;
                    ev.EveryMonth = entity.EveryMonth;
                    ev.EveryWeek = entity.EveryWeek;
                    ev.EveryYear = entity.EveryYear;
                    ev.CalendarID = entity.CalendarID;
                }
            }
            this.unitOfWork.Commit();
        }

        public List<Event> GetEvents(Calendar cal)
        {
            var list = new List<Event>();
            if (cal != null)
            {
                var calendar = this.unitOfWork.Context.Calendars.Find(cal.ID);
                if (calendar.Events.Count > 0)
                {
                    foreach (var item in calendar.Events)
                    {
                        list.Add(DomainToDal.Map(item));
                    }
                }
            }
            return list;
        }
    }
}
