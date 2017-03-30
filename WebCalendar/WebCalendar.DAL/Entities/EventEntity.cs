using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCalendar.DAL.Entities
{
    [Table("Event")]
    public class EventEntity
    {
        public EventEntity()
        {
            this.Notifications = new List<NotificationEntity>();
            this.Occurrences = new List<OccurrenceEntity>();
        }
        [Key]
        public int ID { get; set; }
        public string Title { get; set; }
        [Required(ErrorMessage = "Enter a description!")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Enter a start date!")]
        [Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Enter an end date!")]
        [Column(TypeName = "datetime")]
        public DateTime EndDate { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool EveryDay { get; set; }
        public bool EveryWeek { get; set; }
        public bool EveryMonth { get; set; }
        public bool EveryYear { get; set; }
        public int EventColor { get; set; }
        public int CalendarID { get; set; }
        [ForeignKey("CalendarID")]
        public virtual CalendarEntity Calendar { get; set; }
        public virtual List<NotificationEntity> Notifications { get; set; }
        public virtual List<OccurrenceEntity> Occurrences { get; set; }
    }
}
