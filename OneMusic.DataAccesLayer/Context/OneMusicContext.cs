using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OneMusic.EntityLayer.Entities;
using OneMusic.EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMusic.DataAccessLayer.Context
{
    public class OneMusicContext : IdentityDbContext<AppUser,AppRole,int> // primary Key int değerdir dedik
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=LAPTOP-1IHFPICF;database=OneMusicDb-Re;trusted_connection=true;multipleactiveresultsets=true;trustservercertificate=true;");

        }

        public DbSet<About> Abouts { get; set; } 

        public DbSet<Banner> Banners { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Singer> Singers { get; set; }

        public DbSet<Song> Songs { get; set; }
        public DbSet<Category> Categories { get; set; }


    }
}
