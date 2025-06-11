using OneMusic.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace OneMusic.EntityLayer.Entity
{
    public class Song
    {
        public int SongId { get; set; }

        public string SongName { get; set; }
        public string SongUrl { get; set; }
        public int AlbumId { get; set; }
        public int? SingerId { get; set; }


        public bool IsNewSong { get; set; }=false;
        

 

        public Singer Singer { get; set; }
        public Album Album { get; set; }

        
        
    }
}
