using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WebCalendar.DAL.Context;
using WebCalendar.DAL.Entities;
using WebCalendar.DAL.Mappers;
using WebCalendar.Domain.Aggregate.Calendar;

namespace WebCalendar.DAL.Concrete
{
    public class CalendarRepository : ICalendarRepository
    {
        private readonly UnitOfWork unitOfWork;
        private readonly DbSet<CalendarEntity> dal;

        public CalendarRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null) throw new ArgumentNullException("Null context!");
            this.unitOfWork = (UnitOfWork)unitOfWork;
            this.dal = this.unitOfWork.Context.Calendars;
        }

        public void Add(Calendar cal)
        {
            if (cal == null) throw new ArgumentNullException("entity");
            var entity = new CalendarEntity();
            DomainToDal.Map(entity, cal);
            this.dal.Add(entity);
            this.unitOfWork.Commit();
            this.unitOfWork.Dispose();
        }

        public IQueryable<Calendar> Entities
        {
            get
            {
                var list = new List<Calendar>();
                var cal = unitOfWork.Context.Calendars.ToList();
                foreach (var item in cal)
                {
                    list.Add(DomainToDal.Map(item));
                }
                return list.AsQueryable();
            }
        }

        public void Delete(int id)
        {
            CalendarEntity entity = this.dal.Find(id);
            if (entity == null) throw new ArgumentNullException("entity");
            this.dal.Attach(entity);
            this.dal.Remove(entity);
            this.unitOfWork.Commit();
        }

        public void Update(Calendar entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            var newEntity = new CalendarEntity();
            DomainToDal.Map(newEntity, entity);
            if (entity.ID == Constants.NEW_DATABASE_ID_VALUE)
            {
                dal.Add(newEntity);
            }
            else
            {
                CalendarEntity cal = dal.Find(entity.ID);
                cal.Title = newEntity.Title;
                cal.Description = newEntity.Description;
            }
            this.unitOfWork.Commit();
            this.unitOfWork.Dispose();
        }
    }
}
