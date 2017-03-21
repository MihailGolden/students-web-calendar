using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCalendar.DAL.Entities
{
    [Table("Calendar")]
    public class CalendarEntity
    {
        [Key]
        public int ID { get; set; }
        public CalendarEntity()
        {
            this.Events = new List<EventEntity>();
        }
        [Required(ErrorMessage = "Enter a start date!")]
        public string Title { get; set; }
        public virtual List<EventEntity> Events { get; set; }
    }
}
