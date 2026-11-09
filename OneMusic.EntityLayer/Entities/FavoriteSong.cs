using OneMusic.EntityLayer.Entity;

namespace OneMusic.EntityLayer.Entities
{
    // Kullanıcının favorilere eklediği şarkılar (kullanıcı - şarkı eşleşmesi)
    public class FavoriteSong
    {
        public int FavoriteSongId { get; set; }

        public int AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        public int SongId { get; set; }
        public Song? Song { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
