using OneMusic.EntityLayer.Entities;
using OneMusic.EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMusic.DataAccessLayer.Abstract
{
    public interface IAlbumDal : IGenericDal<Album>
    {

        List<Album> GetAlbumswithArtist();//sanatçılar ile birlikte bütün albümleri getir

        List<Album> GetAlbumsByArtist(int id);//sanatçıyı bulup albümlerini getiriyor 

        List<Album> GetNewAlbums();

    }
}