using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WebCalendar.DAL.Context;
using WebCalendar.DAL.Entities;
using WebCalendar.DAL.Mappers;
using WebCalendar.Domain.Aggregate.Occurrence;

namespace WebCalendar.DAL.Repositories
{
    public class OccurenceRepository : IOccurrenceRepository
    {
        private readonly UnitOfWork unitOfWork;
        private readonly DbSet<OccurrenceEntity> dal;

        public OccurenceRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = (UnitOfWork)unitOfWork;
            this.dal = this.unitOfWork.Context.Occurrences;
        }

        public void Add(Occurrence occur)
        {
            var entity = new OccurrenceEntity();
            DomainToDal.Map(entity, occur);
            this.dal.Add(entity);
            this.unitOfWork.Commit();
            occur.ID = entity.OccurenceID;
        }

        public IQueryable<Occurrence> Entities
        {
            get
            {
                var list = new List<Occurrence>();
                var occurs = unitOfWork.Context.Occurrences.ToList();
                foreach (var item in occurs)
                {
                    list.Add(DomainToDal.Map(item));
                }
                return list.AsQueryable();
            }
        }

        public void Delete(int id)
        {
            OccurrenceEntity entity = this.dal.Find(id);
            if (entity != null)
            {
                this.dal.Attach(entity);
                this.dal.Remove(entity);
                this.unitOfWork.Commit();
            }
        }

        public void Update(Occurrence entity)
        {
            var newEntity = new OccurrenceEntity();
            DomainToDal.Map(newEntity, entity);
            if (entity.ID == 0)
            {
                dal.Add(newEntity);
            }
            else
            {
                OccurrenceEntity occur = dal.Find(entity.ID);
                if (occur != null)
                {
                    occur.Count = newEntity.Count;
                }
            }
            this.unitOfWork.Commit();
        }
    }
}
