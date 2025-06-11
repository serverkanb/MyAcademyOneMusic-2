using OneMusic.DataAccessLayer.Migrations;
using OneMusic.EntityLayer.Entities;
using OneMusic.EntityLayer.Entity;

namespace OneMusic.WebUI.Models
{
    public class SongSingerViewModel
    {
        public List<Album> NewAlbums { get; set; }
        public List<Song> NewSongs { get; set; }
        public List<Singer> NewArtists { get; set; }
    }
}
