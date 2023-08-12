using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IGenericService<T> where T : class,new()
    {
        void Insert(T t);
        void Delete(T t);
        void Update(T t);
        T GetElementById(int id);
        List<T> GetListAll();
    }
}
