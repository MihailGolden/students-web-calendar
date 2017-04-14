using System.Threading.Tasks;

namespace WebCalendar.DAL.Context
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly WebCalendarContext context;
        public UnitOfWork()
        {
            this.context = new WebCalendarContext();
        }

        internal WebCalendarContext Context
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
