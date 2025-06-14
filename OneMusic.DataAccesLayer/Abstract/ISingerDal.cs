﻿using OneMusic.EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMusic.DataAccessLayer.Abstract
{
    public interface ISingerDal : IGenericDal<Singer>
    {
        List<Singer> GetPopularArtist();
    }
}
