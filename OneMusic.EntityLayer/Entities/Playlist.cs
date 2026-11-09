namespace OneMusic.EntityLayer.Entities
{
    // Kullanıcının oluşturduğu çalma listesi
    public class Playlist
    {
        public int PlaylistId { get; set; }

        public string Name { get; set; } = "";

        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        public List<PlaylistSong>? PlaylistSongs { get; set; }
    }
}
