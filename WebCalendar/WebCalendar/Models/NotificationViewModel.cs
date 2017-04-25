using System;

namespace WebCalendar.Models
{
    public class NotificationViewModel
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public int NotificateBeforeDay { get; set; }
        public DateTime? NotificationDefaultTime { get; set; }
        public int? EventID { get; set; }
        public string FieldPrefix { get; set; }
    }
}