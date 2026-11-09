using OneMusic.EntityLayer.Entities;

namespace OneMsuic.BusinessLayer.Abstract
{
    public interface IFavoriteSongService : IGenericService<FavoriteSong>
    {
        List<FavoriteSong> TGetFavoritesByUser(int userId);
        List<int> TGetFavoriteSongIds(int userId);

        // Şarkı favorideyse çıkarır, değilse ekler. Yeni durumu döner (true = favoride).
        bool TToggleFavorite(int userId, int songId);
    }
}
