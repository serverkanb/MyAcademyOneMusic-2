using OneMusic.EntityLayer.Entity;

namespace OneMusic.EntityLayer.Entities
{
    // Çalma listesi - şarkı eşleşmesi (bir şarkı bir listede bir kez bulunur)
    public class PlaylistSong
    {
        public int PlaylistSongId { get; set; }

        public int PlaylistId { get; set; }
        public Playlist? Playlist { get; set; }

        public int SongId { get; set; }
        public Song? Song { get; set; }

        public DateTime AddedDate { get; set; } = DateTime.Now;
    }
}
