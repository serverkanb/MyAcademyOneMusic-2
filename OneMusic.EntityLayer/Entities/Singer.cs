using OneMusic.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMusic.EntityLayer.Entity
{
    public class Singer
    {
        public int SingerId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public string? About { get; set; }

        public bool IsPopularArtist { get; set; } = false;

        public List<Song> Songs { get; set; }

        public int? AppUserId { get; set; } // Foreign Key olarak AppUserId eklendi
        public AppUser AppUser { get; set; }

    }
}
