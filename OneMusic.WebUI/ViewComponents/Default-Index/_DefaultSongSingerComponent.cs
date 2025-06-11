using Microsoft.AspNetCore.Mvc;
using OneMsuic.BusinessLayer.Abstract;
using OneMusic.DataAccessLayer.Abstract;
using OneMusic.DataAccessLayer.Migrations;
using OneMusic.EntityLayer.Entity;
using OneMusic.WebUI.Models;

namespace OneMusic.WebUI.ViewComponents.Default_Index
{
    public class _DefaultSongSingerComponent : ViewComponent
    {
        private readonly ISongService _songService;
        private readonly ISingerService _singerService;
        private readonly IAlbumService _albumService;

        public _DefaultSongSingerComponent(ISongService songService, ISingerService singerService, IAlbumService albumService)
        {
            _songService = songService;
            _singerService = singerService;
            _albumService = albumService;
        }

        public IViewComponentResult Invoke()
        {
            var model = new SongSingerViewModel
            {
                NewAlbums = _albumService.TGetNewAlbums(),
                NewSongs = _songService.TGetNewSongs(),
                NewArtists = _singerService.TGetPopularArtists(),
            };

            return View(model);
        }
    }


}

