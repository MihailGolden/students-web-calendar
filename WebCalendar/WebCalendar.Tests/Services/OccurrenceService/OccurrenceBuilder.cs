using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using WebCalendar.Domain.Aggregate.Occurrence;

namespace WebCalendar.Tests.Services.OccurrenceService
{
    [ExcludeFromCodeCoverage]
    public class OccurrenceBuilder
    {
        private Occurrence occurrence;
        private List<Occurrence> occurrences;
        public OccurrenceBuilder()
        {
            this.occurrence = new Occurrence()
            {
                ID = 1,
                Count = 1
            };
            this.occurrences = new List<Occurrence>() { occurrence };
        }
        public Occurrence Build()
        {
            return this.occurrence;
        }
        public List<Occurrence> BuildList()
        {
            return this.occurrences;
        }
    }
}
