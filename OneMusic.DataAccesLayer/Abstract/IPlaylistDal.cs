using OneMusic.EntityLayer.Entities;

namespace OneMusic.DataAccessLayer.Abstract
{
    public interface IPlaylistDal : IGenericDal<Playlist>
    {
        List<Playlist> GetPlaylistsByUser(int userId); // şarkı sayısı için PlaylistSongs ile
        Playlist? GetPlaylistWithSongs(int playlistId); // şarkı, albüm ve sanatçı bilgileriyle
        List<int> GetPlaylistIdsContainingSong(int userId, int songId);
        PlaylistSong? GetPlaylistSong(int playlistId, int songId);
        void AddSong(int playlistId, int songId);
        void RemoveSong(int playlistId, int songId);
    }
}
