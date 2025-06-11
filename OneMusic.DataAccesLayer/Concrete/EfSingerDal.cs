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
    public class EfSingerDal : GenericRepository<Singer>, ISingerDal
    {
        private readonly OneMusicContext _context;
        public EfSingerDal(OneMusicContext context) : base(context)
        {
            _context = context;
        }

        public List<Singer> GetPopularArtist()
        {
            return _context.Singers.Where(x=>x.IsPopularArtist == true).ToList();
        }
    }
}
