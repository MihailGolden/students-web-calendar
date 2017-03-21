using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCalendar.DAL.Context
{
   public interface IUnitOfWork:IDisposable
    {
        void Commit();
        Task CommitAsync();
    }
}
