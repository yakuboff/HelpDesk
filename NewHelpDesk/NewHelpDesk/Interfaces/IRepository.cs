using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewHelpDesk.Interfaces
{
    public interface IRepository<T> where T:class
    {
        void Create(T item);
        void Delete(int id);
        void Update(T item);
        T Get(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Func<T, Boolean> predicate);       
    }
}
