using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMusic.DataAccessLayer.Abstract
{
    //
     public interface IGenericDal<T> where T : class
    {
        // crud işlemlerini yaptığımız yer 
        List<T> GetList();
        T GetById(int id);

        void Create(T entity);

        void Update(T entity);

        void Delete(int id);

    }
}
