using System.Collections.Generic;
using WebCalendar.Domain.Aggregate.Occurrence;

namespace WebCalendar.Contracts
{
    public interface IOccurrenceService
    {
        void Create(Occurrence occurrence);
        void AddOccurrences(List<Occurrence> occurrences);
        Occurrence Get(int id);
        List<Occurrence> GetOccurrences { get; }
        void Update(Occurrence occurrence);
        void Delete(int id);
    }
}
