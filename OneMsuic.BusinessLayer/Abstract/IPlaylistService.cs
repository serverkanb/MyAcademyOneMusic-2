using OneMusic.EntityLayer.Entities;

namespace OneMsuic.BusinessLayer.Abstract
{
    public interface IPlaylistService : IGenericService<Playlist>
    {
        List<Playlist> TGetPlaylistsByUser(int userId);

        // Liste bu kullanıcıya ait değilse null döner
        Playlist? TGetUserPlaylist(int userId, int playlistId);
        Playlist? TGetUserPlaylistWithSongs(int userId, int playlistId);

        List<int> TGetPlaylistIdsContainingSong(int userId, int songId);

        // Şarkı listedeyse çıkarır, değilse ekler. Yeni durumu döner (true = listede).
        // Liste kullanıcıya ait değilse null döner.
        bool? TToggleSong(int userId, int playlistId, int songId);

        // Liste kullanıcıya ait değilse false döner
        bool TRemoveSong(int userId, int playlistId, int songId);
    }
}
