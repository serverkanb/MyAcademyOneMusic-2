using OneMusic.EntityLayer.Entities;
using OneMusic.EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMsuic.BusinessLayer.Abstract
{
    public interface IAlbumService : IGenericService<Album>
    {
        List<Album> TGetAlbumsByArtist(int id);

        public List<Album> TGetAlbumswithArtist();

        public List<Album> TGetNewAlbums();
    }
}
