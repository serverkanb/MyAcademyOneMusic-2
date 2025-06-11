using Microsoft.EntityFrameworkCore;
using OneMusic.DataAccessLayer.Abstract;
using OneMusic.DataAccessLayer.Context;
using OneMusic.DataAccessLayer.Repositories;
using OneMusic.EntityLayer.Entities;
using OneMusic.EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMusic.DataAccessLayer.Concrete
{
    public class EfSongDal : GenericRepository<Song>, ISongDal
    {
        private readonly OneMusicContext _contex;

        public EfSongDal(OneMusicContext context) : base(context)
        {
            _contex = context;
        }

        public List<Song> GetNewSongs()
        {
            return _contex.Songs.Where(x => x.IsNewSong == true).OrderByDescending(x => x.SongId).Take(6).ToList();
        }

        public List<Song> GetSongsWithAlbumAndArtist()
        {
            //Include da bağlı tabloyu then include ile onunda bağlı olduğu tabloyu 
            return _contex.Songs.Include(x => x.Album).Include(x=>x.Singer).Include(x=>x.Singer).ToList();
        }

        public List<Song> GetSongsWithAlbumByUserId(int id)
        {
            return _contex.Songs.Include(x=>x.Album).ThenInclude(x=>x.AppUser).Where(x=>x.Album.AppUserId==id).ToList();
        }

       
    }
}
