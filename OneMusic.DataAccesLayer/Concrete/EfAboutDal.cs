using OneMusic.DataAccessLayer.Abstract;
using OneMusic.DataAccessLayer.Context;
using OneMusic.DataAccessLayer.Repositories;
using OneMusic.EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMusic.DataAccessLayer.Concrete
{
    //Abstract soyut ifadeleri tutuyor concrete de sınıfları tutan
    public class EfAboutDal : GenericRepository<About>, IAboutDal
    {
        public EfAboutDal(OneMusicContext context) : base(context)
        {

        }
    }
}
