using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewHelpDesk.Interfaces
{
    public interface IClassifier<TEntity> where TEntity:class
    {
        IEnumerable<TEntity> GetClassifier();
    }
}
