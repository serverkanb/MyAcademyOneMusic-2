using OneMusic.EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMusic.DataAccessLayer.Abstract
{
    public interface ISongDal : IGenericDal<Song>
    {
        List<Song>  GetSongsWithAlbumAndArtist();

        List<Song> GetSongsWithAlbumByUserId(int id);



        List<Song> GetNewSongs();

    }
}
