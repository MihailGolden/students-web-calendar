using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCalendar.DAL.Entities
{
    [Table("Event")]
    public class EventEntity
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Enter a description!")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Enter a start date!")]
        [Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Enter an end date!")]
        [Column(TypeName = "datetime")]
        public DateTime EndDate { get; set; }
        public int CalendarID { get; set; }
        public virtual CalendarEntity Calendar { get; set; }
    }
}
