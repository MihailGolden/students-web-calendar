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
        }
        [Key]
        public int EventID { get; set; }
        [Required(ErrorMessage = "Enter a title!")]
        public string Title { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool? EveryDay { get; set; }
        public bool? EveryWeek { get; set; }
        public bool? EveryMonth { get; set; }
        public bool? EveryYear { get; set; }
        public int EventColor { get; set; }
        public int CalendarID { get; set; }
        [ForeignKey("CalendarID")]
        public virtual CalendarEntity Calendar { get; set; }
        public int? OccurrenceID { get; set; }
        [ForeignKey("OccurrenceID")]
        public virtual OccurrenceEntity Occurrence { get; set; }
        public virtual List<NotificationEntity> Notifications { get; set; }
    }
}
