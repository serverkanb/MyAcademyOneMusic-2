using OneMsuic.BusinessLayer.Abstract;
using OneMusic.DataAccessLayer.Abstract;
using OneMusic.EntityLayer.Entities;

namespace OneMsuic.BusinessLayer.Concrete
{
    public class FavoriteSongManager : IFavoriteSongService
    {
        private readonly IFavoriteSongDal _favoriteSongDal;

        public FavoriteSongManager(IFavoriteSongDal favoriteSongDal)
        {
            _favoriteSongDal = favoriteSongDal;
        }

        public void TCreate(FavoriteSong entity)
        {
            _favoriteSongDal.Create(entity);
        }

        public void TDelete(int id)
        {
            _favoriteSongDal.Delete(id);
        }

        public FavoriteSong? TGetById(int id)
        {
            return _favoriteSongDal.GetById(id);
        }

        public List<FavoriteSong> TGetFavoritesByUser(int userId)
        {
            return _favoriteSongDal.GetFavoritesByUser(userId);
        }

        public List<int> TGetFavoriteSongIds(int userId)
        {
            return _favoriteSongDal.GetFavoriteSongIds(userId);
        }

        public List<FavoriteSong> TGetList()
        {
            return _favoriteSongDal.GetList();
        }

        public bool TToggleFavorite(int userId, int songId)
        {
            var existing = _favoriteSongDal.GetByUserAndSong(userId, songId);
            if (existing != null)
            {
                _favoriteSongDal.Delete(existing.FavoriteSongId);
                return false;
            }

            _favoriteSongDal.Create(new FavoriteSong { AppUserId = userId, SongId = songId });
            return true;
        }

        public void TUpdate(FavoriteSong entity)
        {
            _favoriteSongDal.Update(entity);
        }
    }
}
