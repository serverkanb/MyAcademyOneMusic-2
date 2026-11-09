using OneMusic.EntityLayer.Entities;

namespace OneMusic.DataAccessLayer.Abstract
{
    public interface IFavoriteSongDal : IGenericDal<FavoriteSong>
    {
        List<FavoriteSong> GetFavoritesByUser(int userId); // şarkı, albüm ve sanatçı bilgileriyle
        List<int> GetFavoriteSongIds(int userId);
        FavoriteSong? GetByUserAndSong(int userId, int songId);
    }
}
