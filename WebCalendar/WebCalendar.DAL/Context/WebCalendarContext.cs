using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
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
            modelBuilder.Entity<NotificationEntity>().HasOptional(e => e.Event).WithMany(n => n.Notifications);
            modelBuilder.Entity<EventEntity>().HasOptional(e => e.Occurrence).WithMany(o => o.Events);
            modelBuilder.Entity<CalendarEntity>().Property(c => c.UserID).HasMaxLength(128);
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException exception)
            {
                foreach (var validationErrors in exception.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Class: {0}, Property: {1}, Error: {2}",
                            validationErrors.Entry.Entity.GetType().FullName,
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }

                string errorMessages = string.Join("; ", exception.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }
        }
    }
}

