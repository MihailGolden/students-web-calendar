using System.Data.Entity;
using WebCalendar.DAL.Entities;

namespace WebCalendar.DAL.Context
{
    public class WebCalendarContext : DbContext
    {
        public WebCalendarContext() : base("WebCalendarDb")
        {

        }
        public DbSet<EventEntity> Events { get; set; }
        public DbSet<CalendarEntity> Calendars { get; set; }
        public DbSet<NotificationEntity> Notifications { get; set; }
        public DbSet<OccurrenceEntity> Occurrences { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // relationship 1 to many 
            modelBuilder.Entity<EventEntity>().HasRequired(c => c.Calendar).WithMany(e => e.Events);
            modelBuilder.Entity<NotificationEntity>().HasRequired(e => e.Event).WithMany(n => n.Notifications);
            modelBuilder.Entity<OccurrenceEntity>().HasRequired(e => e.Event).WithMany(o => o.Occurrences);
        }
    }
}
