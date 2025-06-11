using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OneMsuic.BusinessLayer.Abstract;
using OneMusic.EntityLayer.Entities;
using OneMusic.EntityLayer.Entity;
using OneMusic.WebUI.Areas.Artist.Models;

namespace OneMusic.WebUI.Areas.Artist.Controllers
{
    [Area("Artist")]
    [Authorize(Roles = "Artist")]
    [Route("[area]/[controller]/[action]/{id?}")]
    public class MySongController : Controller
    {
        private readonly ISongService _songService;
        private readonly IAlbumService _albumService;
        private readonly UserManager<AppUser> _userManager;

        public MySongController(ISongService songService, IAlbumService albumService, UserManager<AppUser> userManager)
        {
            _songService = songService;
            _albumService = albumService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var values = _songService.TGetSongsWithAlbumByUserId(user.Id);

            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> CreateSong()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var albumList = _albumService.TGetAlbumsByArtist(user.Id);

            List<SelectListItem> albums = (from x in albumList
                                           select new SelectListItem
                                           {
                                               Text = x.AlbumName,
                                               Value = x.AlbumId.ToString()
                                           }).ToList();
            ViewBag.Albums = albums;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSong(SongViewModel model)
        {
            var song = new Song
            {
                SongName = model.SongName,
                AlbumId = model.AlbumId,
            };
            if (model.SongFile != null)
            {
                var resource = Directory.GetCurrentDirectory();
                var extension = Path.GetExtension(model.SongFile.FileName).ToLower();
                if (extension != ".mp3")
                {
                    // Desteklenmeyen dosya uzantısı hatası
                    ModelState.AddModelError("SongFile", "Sadece mp3.");
                    // Gerekirse, işlemi sonlandırabilirsiniz.
                    return View(model);
                }
                var songName = Guid.NewGuid() + extension;
                var saveLocation = resource + "/wwwroot/songs/" + songName;
                var stream = new FileStream(saveLocation, FileMode.Create);
                await model.SongFile.CopyToAsync(stream);
                song.SongUrl = "/songs/" + songName;
            }

            _songService.TCreate(song);


            return RedirectToAction("Index");
        }

        public IActionResult DeleteSong(int id)
        {
            _songService.TDelete(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult UpdateSong(int id)
        {
            var values = _songService.TGetById(id);
            return View(values);
        }





    }
}
