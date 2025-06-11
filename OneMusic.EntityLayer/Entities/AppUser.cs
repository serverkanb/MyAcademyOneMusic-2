using Microsoft.AspNetCore.Identity;
using OneMusic.EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMusic.EntityLayer.Entities
{
    public class AppUser : IdentityUser<int>
    {
       public string Name {  get; set; }
       public string SurName {  get; set; }
       public string? ImageUrl {  get; set; }

       public List<Album> Albums {  get; set; }

        public Singer? Singer { get; set; }

  





    }
}
