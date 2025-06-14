﻿using OneMusic.EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMusic.EntityLayer.Entities
{
    public class Album
    {
        public int AlbumId { get; set; }
        public string AlbumName { get; set; }
        public string CoverImage { get; set; }
        public decimal Price { get; set; }
        public bool IsNewAlbum { get; set; } = false;

        public DateTime? RelaseDate { get; set; }

        public int? AppUserId { get; set; }

        public AppUser AppUser { get; set; }

        public List<Song> Songs { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public int? SingerId { get; set; }
        public Singer Singer { get; set; }
    }
}
