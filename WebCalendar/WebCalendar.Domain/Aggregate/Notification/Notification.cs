using System;
using WebCalendar.Domain.Properties;

namespace WebCalendar.Domain.Aggregate.Notification
{
    public class Notification
    {
        private string type;
        public int ID { get; set; }
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                if (NotificationValidation.ValidateType(value))
                {
                    throw new ArgumentException(Resources.ValidationType, "Type");
                }
                this.type = value;
            }
        }
        public int NotificateBeforeDay { get; set; }
        public DateTime NotificationDefaultTime { get; set; }
        public int? EventID { get; set; }
    }
}
