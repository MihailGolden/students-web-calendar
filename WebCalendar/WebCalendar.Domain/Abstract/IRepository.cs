using System.Linq;

namespace WebCalendar.Domain.Abstract
{
    public interface IRepository<T>
    {
        IQueryable<T> Entities { get; }
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
