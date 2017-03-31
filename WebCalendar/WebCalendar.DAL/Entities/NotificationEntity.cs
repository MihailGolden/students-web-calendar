using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCalendar.DAL.Entities
{
    [Table("Notification")]
    public class NotificationEntity
    {
        [Key]
        public int NotificationID { get; set; }
        public string Type { get; set; }
        public int NotificateBeforeDay { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime NotificationDefaultTime { get; set; }
        public int? EventID { get; set; }
        public virtual EventEntity Event { get; set; }
    }
}
