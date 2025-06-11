using OneMusic.DataAccessLayer.Abstract;
using OneMusic.DataAccessLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMusic.DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        //Constructer yapısı kullanıyoruz 
        //constructer yapısı bir sınıfı new lediğimiz ilk kez çalıştırdığımız zaman kullanacağımız yapıdır
        //ctor yazıyoruz

        private readonly OneMusicContext _context;

        public GenericRepository(OneMusicContext context)
        {
            _context = context;
        }

        public void Create(T entity)//T türünde bir entity diye zaten kendi belirtmiş 
        {
           _context.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var value = _context.Set<T>().Find(id);
            _context.Set<T>().Remove(value);
            _context.SaveChanges();

        }

       
        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public List<T> GetList()
        {
           return _context.Set<T>().ToList();
        }

        public void Update(T entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }
    }
}
