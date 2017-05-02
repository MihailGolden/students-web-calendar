using System;
using System.Collections.Generic;
using System.Linq;
using WebCalendar.Contracts;
using WebCalendar.Domain.Aggregate.Occurrence;
using WebCalendar.Domain.Exceptions;
using WebCalendar.Services.Resources;

namespace WebCalendar.Services
{
    public class OccurrenceService : IOccurrenceService
    {
        IOccurrenceRepository occurrenceRepository;
        public OccurrenceService(IOccurrenceRepository occurrenceRepository)
        {
            this.occurrenceRepository = occurrenceRepository;
        }
        public List<Occurrence> GetOccurrences
        {
            get
            {
                return this.occurrenceRepository.Entities.ToList();
            }
        }

        public void AddOccurrences(List<Occurrence> occurrences)
        {
            if (occurrences != null)
            {
                foreach (var item in occurrences)
                {
                    this.occurrenceRepository.Add(item);
                }
            }
            else
            {
                throw new ArgumentNullException("Null occurrences!");
            }
        }

        public void Create(Occurrence occurrence)
        {
            if (occurrence == null)
            {
                throw new ArgumentNullException("Null occurrence!");
            }
            if (occurrence != null)
            {
                this.occurrenceRepository.Add(occurrence);
            }
        }

        public void Delete(int id)
        {
            var occurrence = this.occurrenceRepository.Entities.FirstOrDefault(o => o.ID == id);
            if (occurrence == null)
            {
                throw new ArgumentNullException("Null occurrence!");
            }
            this.occurrenceRepository.Delete(id);
        }

        public Occurrence Get(int id)
        {
            var occurrence = this.occurrenceRepository.Entities.FirstOrDefault(o => o.ID == id);
            return occurrence;
        }

        public void Update(Occurrence occurrence)
        {
            try
            {
                this.occurrenceRepository.Update(occurrence);
            }
            catch (ArgumentNullException ex)
            {
                throw new ConversionException(ServiceResource.OccurrenceNotFound, ex);
            }
        }
    }
}
