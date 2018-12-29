using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryWpfRest.Interfaces
{
    interface IRepository<Mobile>
    {
        IEnumerable<Mobile> GetAll();
        Mobile Get(string name);
        void Create(Mobile t);
        void Update(Mobile t);
        void Delete(IEnumerable<Mobile> t);
    }
}
