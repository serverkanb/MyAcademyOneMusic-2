using OneMusic.DataAccessLayer.Migrations;
using OneMusic.EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMsuic.BusinessLayer.Abstract
{
    public interface ISingerService : IGenericService<Singer>
    {
        List<Singer> TGetPopularArtists();
    }
}
