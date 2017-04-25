using System;
using System.ComponentModel.DataAnnotations;

namespace WebCalendar.Models
{
    public class NotificationViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Enter type")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Enter time before day")]
        public int NotificateBeforeDay { get; set; }
        public DateTime? NotificationDefaultTime { get; set; }
        public int? EventID { get; set; }
        public string FieldPrefix { get; set; }
    }
}