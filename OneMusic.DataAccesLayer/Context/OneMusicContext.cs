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
        // Bağlantı cümlesi artık appsettings.json -> ConnectionStrings:DefaultConnection içinden
        // Program.cs'deki AddDbContext ile veriliyor.
        public OneMusicContext(DbContextOptions<OneMusicContext> options) : base(options)
        {
        }

        public DbSet<About> Abouts { get; set; } 

        public DbSet<Banner> Banners { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Singer> Singers { get; set; }

        public DbSet<Song> Songs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<FavoriteSong> FavoriteSongs { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistSong> PlaylistSongs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Identity tabloları için gerekli

            // Bir kullanıcı aynı şarkıyı sadece bir kez favoriye ekleyebilir
            builder.Entity<FavoriteSong>()
                .HasIndex(x => new { x.AppUserId, x.SongId })
                .IsUnique();

            builder.Entity<Playlist>()
                .Property(x => x.Name)
                .HasMaxLength(100);

            builder.Entity<Playlist>()
                .Property(x => x.Description)
                .HasMaxLength(500);

            // Aynı şarkı bir listeye sadece bir kez eklenebilir
            builder.Entity<PlaylistSong>()
                .HasIndex(x => new { x.PlaylistId, x.SongId })
                .IsUnique();
        }


    }
}
