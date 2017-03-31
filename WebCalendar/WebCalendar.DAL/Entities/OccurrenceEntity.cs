using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCalendar.DAL.Entities
{
    [Table("Occurence")]
    public class OccurrenceEntity
    {
        public OccurrenceEntity()
        {
            this.Events = new List<EventEntity>();
        }
        [Key]
        public int OccurenceID { get; set; }
        public int Count { get; set; }
        public virtual List<EventEntity> Events { get; set; }
    }
}
