using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCalendar.DAL.Entities
{
    [Table("Notification")]
    public class NotificationEntity
    {
        [Key]
        public int ID { get; set; }
        public string Type { get; set; }
        public int EventID { get; set; }
        public virtual EventEntity Event { get; set; }
    }
}
