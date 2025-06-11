using OneMusic.EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMsuic.BusinessLayer.Abstract
{
    public interface ISongService : IGenericService<Song>
    {
        public List<Song> TGetSongsWithAlbumAndArtist();

        List<Song> TGetSongsWithAlbumByUserId(int id);

        List<Song> TGetNewSongs();
    }
}
 