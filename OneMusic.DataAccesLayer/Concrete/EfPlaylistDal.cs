using Microsoft.EntityFrameworkCore;
using OneMusic.DataAccessLayer.Abstract;
using OneMusic.DataAccessLayer.Context;
using OneMusic.DataAccessLayer.Repositories;
using OneMusic.EntityLayer.Entities;

namespace OneMusic.DataAccessLayer.Concrete
{
    public class EfPlaylistDal : GenericRepository<Playlist>, IPlaylistDal
    {
        private readonly OneMusicContext _context;

        public EfPlaylistDal(OneMusicContext context) : base(context)
        {
            _context = context;
        }

        public List<Playlist> GetPlaylistsByUser(int userId)
        {
            return _context.Playlists
                .Include(x => x.PlaylistSongs)
                .Where(x => x.AppUserId == userId)
                .OrderByDescending(x => x.CreatedDate)
                .ToList();
        }

        public Playlist? GetPlaylistWithSongs(int playlistId)
        {
            return _context.Playlists
                .Include(x => x.PlaylistSongs!)
                    .ThenInclude(x => x.Song).ThenInclude(x => x!.Album).ThenInclude(x => x!.Singer)
                .Include(x => x.PlaylistSongs!)
                    .ThenInclude(x => x.Song).ThenInclude(x => x!.Singer)
                .FirstOrDefault(x => x.PlaylistId == playlistId);
        }

        public List<int> GetPlaylistIdsContainingSong(int userId, int songId)
        {
            return _context.PlaylistSongs
                .Where(x => x.SongId == songId && x.Playlist!.AppUserId == userId)
                .Select(x => x.PlaylistId)
                .ToList();
        }

        public PlaylistSong? GetPlaylistSong(int playlistId, int songId)
        {
            return _context.PlaylistSongs.FirstOrDefault(x => x.PlaylistId == playlistId && x.SongId == songId);
        }

        public void AddSong(int playlistId, int songId)
        {
            _context.PlaylistSongs.Add(new PlaylistSong { PlaylistId = playlistId, SongId = songId });
            _context.SaveChanges();
        }

        public void RemoveSong(int playlistId, int songId)
        {
            var value = GetPlaylistSong(playlistId, songId);
            if (value != null)
            {
                _context.PlaylistSongs.Remove(value);
                _context.SaveChanges();
            }
        }
    }
}
