using System;
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
            if (unitOfWork == null) throw new ArgumentNullException("Null context!");
            this.unitOfWork = (UnitOfWork)unitOfWork;
            this.dal = this.unitOfWork.Context.Occurrences;
        }

        public void Add(Occurrence occur)
        {
            if (occur == null) throw new ArgumentNullException("entity");
            var entity = new OccurrenceEntity();
            DomainToDal.Map(entity, occur);
            this.dal.Add(entity);
            this.unitOfWork.Commit();
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
            if (entity == null) throw new ArgumentNullException("entity");
            this.dal.Attach(entity);
            this.dal.Remove(entity);
            this.unitOfWork.Commit();
        }

        public void Update(Occurrence entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            var newEntity = new OccurrenceEntity();
            DomainToDal.Map(newEntity, entity);
            if (entity.ID == Constants.NEW_DATABASE_ID_VALUE)
            {
                dal.Add(newEntity);
            }
            else
            {
                OccurrenceEntity occur = dal.Find(entity.ID);
                occur.Count = newEntity.Count;
            }
            this.unitOfWork.Commit();
            this.unitOfWork.Dispose();
        }
    }
}
