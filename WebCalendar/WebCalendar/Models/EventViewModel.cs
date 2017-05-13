using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebCalendar.Models
{
    public class EventViewModel
    {
        public EventViewModel()
        {
            this.Notifications = new List<NotificationViewModel>();
        }
        public int ID { get; set; }
        [Required(ErrorMessage = "Enter Title")]
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string Color { get; set; }
        [Required(ErrorMessage = "Enter Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd H:mm}")]
        public DateTime BeginTime { get; set; }
        [Required(ErrorMessage = "Enter End Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd H:mm}")]
        public DateTime? EndTime { get; set; }
        public string CalendarTitle { get; set; }
        public int CalendarID { get; set; }
        public IQueryable Calendars { get; set; }
        public List<NotificationViewModel> Notifications { get; set; }
        public bool Repeat { get; set; }
        public int? OccurrenceID { get; set; }
    }
}