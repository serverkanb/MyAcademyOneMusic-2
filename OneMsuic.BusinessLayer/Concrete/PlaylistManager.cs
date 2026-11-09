using OneMsuic.BusinessLayer.Abstract;
using OneMusic.DataAccessLayer.Abstract;
using OneMusic.EntityLayer.Entities;

namespace OneMsuic.BusinessLayer.Concrete
{
    public class PlaylistManager : IPlaylistService
    {
        private readonly IPlaylistDal _playlistDal;

        public PlaylistManager(IPlaylistDal playlistDal)
        {
            _playlistDal = playlistDal;
        }

        public void TCreate(Playlist entity)
        {
            _playlistDal.Create(entity);
        }

        public void TDelete(int id)
        {
            _playlistDal.Delete(id);
        }

        public Playlist? TGetById(int id)
        {
            return _playlistDal.GetById(id);
        }

        public List<Playlist> TGetList()
        {
            return _playlistDal.GetList();
        }

        public void TUpdate(Playlist entity)
        {
            _playlistDal.Update(entity);
        }

        public List<Playlist> TGetPlaylistsByUser(int userId)
        {
            return _playlistDal.GetPlaylistsByUser(userId);
        }

        public Playlist? TGetUserPlaylist(int userId, int playlistId)
        {
            var playlist = _playlistDal.GetById(playlistId);
            return playlist != null && playlist.AppUserId == userId ? playlist : null;
        }

        public Playlist? TGetUserPlaylistWithSongs(int userId, int playlistId)
        {
            var playlist = _playlistDal.GetPlaylistWithSongs(playlistId);
            return playlist != null && playlist.AppUserId == userId ? playlist : null;
        }

        public List<int> TGetPlaylistIdsContainingSong(int userId, int songId)
        {
            return _playlistDal.GetPlaylistIdsContainingSong(userId, songId);
        }

        public bool? TToggleSong(int userId, int playlistId, int songId)
        {
            if (TGetUserPlaylist(userId, playlistId) == null)
            {
                return null;
            }
            if (_playlistDal.GetPlaylistSong(playlistId, songId) != null)
            {
                _playlistDal.RemoveSong(playlistId, songId);
                return false;
            }
            _playlistDal.AddSong(playlistId, songId);
            return true;
        }

        public bool TRemoveSong(int userId, int playlistId, int songId)
        {
            if (TGetUserPlaylist(userId, playlistId) == null)
            {
                return false;
            }
            _playlistDal.RemoveSong(playlistId, songId);
            return true;
        }
    }
}
