﻿using System;
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
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy H:mm}")]
        [DataType(DataType.DateTime)]
        public DateTime BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string CalendarTitle { get; set; }
        public int CalendarID { get; set; }
        public IQueryable Calendars { get; set; }
        public List<NotificationViewModel> Notifications { get; set; }
    }
}