using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCalendar.Domain.Abstract
{
    public interface IRepository<T>
    {
        IQueryable<T> Entities { get; }
        void Update(T entity);
        void Delete(int id);
    }
}
