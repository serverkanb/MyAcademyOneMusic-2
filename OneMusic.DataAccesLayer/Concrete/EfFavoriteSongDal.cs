using Microsoft.EntityFrameworkCore;
using OneMusic.DataAccessLayer.Abstract;
using OneMusic.DataAccessLayer.Context;
using OneMusic.DataAccessLayer.Repositories;
using OneMusic.EntityLayer.Entities;

namespace OneMusic.DataAccessLayer.Concrete
{
    public class EfFavoriteSongDal : GenericRepository<FavoriteSong>, IFavoriteSongDal
    {
        private readonly OneMusicContext _context;

        public EfFavoriteSongDal(OneMusicContext context) : base(context)
        {
            _context = context;
        }

        public List<FavoriteSong> GetFavoritesByUser(int userId)
        {
            return _context.FavoriteSongs
                .Include(x => x.Song).ThenInclude(x => x!.Album).ThenInclude(x => x!.Singer)
                .Include(x => x.Song).ThenInclude(x => x!.Singer)
                .Where(x => x.AppUserId == userId)
                .OrderByDescending(x => x.CreatedDate)
                .ToList();
        }

        public List<int> GetFavoriteSongIds(int userId)
        {
            return _context.FavoriteSongs.Where(x => x.AppUserId == userId).Select(x => x.SongId).ToList();
        }

        public FavoriteSong? GetByUserAndSong(int userId, int songId)
        {
            return _context.FavoriteSongs.FirstOrDefault(x => x.AppUserId == userId && x.SongId == songId);
        }
    }
}
