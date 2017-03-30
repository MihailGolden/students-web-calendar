using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCalendar.DAL.Entities
{
    [Table("Occurence")]
    public class OccurrenceEntity
    {
        [Key]
        public int ID { get; set; }
        public int Count { get; set; }
        public int? EventID { get; set; }
        public virtual EventEntity Event { get; set; }
    }
}
