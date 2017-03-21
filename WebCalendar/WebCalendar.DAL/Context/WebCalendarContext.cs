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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // relationship 1 to many 
            modelBuilder.Entity<EventEntity>().HasRequired(c => c.Calendar).WithMany(e => e.Events);
        }
    }
}
