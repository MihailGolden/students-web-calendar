using System.Threading.Tasks;

namespace WebCalendar.DAL.Context
{
    //To-Do change class to internal and contex too.
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WebCalendarContext context;
        public UnitOfWork()
        {
            this.context = new WebCalendarContext();
        }

        public WebCalendarContext Context
        {
            get { return this.context; }
        }

        public void Commit()
        {
            this.context.SaveChanges();
        }

        public Task CommitAsync()
        {
            return this.context.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}
