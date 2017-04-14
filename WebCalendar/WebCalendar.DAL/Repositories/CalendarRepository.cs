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
            this.unitOfWork = (UnitOfWork)unitOfWork;
            this.dal = this.unitOfWork.Context.Calendars;
        }

        public void Add(Calendar cal)
        {
            var entity = new CalendarEntity();
            DomainToDal.Map(entity, cal);
            this.dal.Add(entity);
            this.unitOfWork.Commit();
            cal.ID = entity.CalendarID;
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
            if (entity != null)
            {
                this.dal.Attach(entity);
                this.dal.Remove(entity);
                this.unitOfWork.Commit();
            }
        }

        public void Update(Calendar entity)
        {
            var newEntity = new CalendarEntity();
            DomainToDal.Map(newEntity, entity);
            if (entity.ID == 0)
            {
                dal.Add(newEntity);
            }
            else
            {
                CalendarEntity cal = dal.Find(entity.ID);
                if (cal != null)
                {
                    cal.Title = newEntity.Title;
                    cal.Description = newEntity.Description;
                    cal.Date = newEntity.Date;
                }
            }
            this.unitOfWork.Commit();
        }
    }
}
