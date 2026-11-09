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
        private readonly ISingerService _singerService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public MySongController(ISongService songService, IAlbumService albumService, ISingerService singerService, UserManager<AppUser> userManager, IWebHostEnvironment environment)
        {
            _songService = songService;
            _albumService = albumService;
            _singerService = singerService;
            _userManager = userManager;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
            var values = _songService.TGetSongsWithAlbumByUserId(user.Id);
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> CreateSong()
        {
            var user = await _userManager.GetUserAsync(User);
            LoadAlbums(user!.Id, null);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSong(SongViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }

            // Şarkı sadece sanatçının kendi albümüne eklenebilir
            if (!IsOwnAlbum(model.AlbumId, user.Id))
            {
                ModelState.AddModelError("AlbumId", "Lütfen kendi albümlerinizden birini seçiniz.");
            }
            if (string.IsNullOrWhiteSpace(model.SongName))
            {
                ModelState.AddModelError("SongName", "Şarkı adı boş bırakılamaz.");
            }
            if (model.SongFile != null && Path.GetExtension(model.SongFile.FileName).ToLower() != ".mp3")
            {
                ModelState.AddModelError("SongFile", "Sadece mp3 dosyaları kabul edilir.");
            }
            if (ModelState.ErrorCount > 0)
            {
                LoadAlbums(user.Id, model.AlbumId);
                return View(model);
            }

            var song = new Song
            {
                SongName = model.SongName,
                AlbumId = model.AlbumId,
                SingerId = _singerService.TGetList().FirstOrDefault(x => x.AppUserId == user.Id)?.SingerId,
            };

            if (model.SongFile != null)
            {
                song.SongUrl = await SaveSongFileAsync(model.SongFile);
            }

            _songService.TCreate(song);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteSong(int id)
        {
            var song = await GetOwnSongAsync(id);
            if (song == null)
            {
                return NotFound();
            }
            _songService.TDelete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSong(int id)
        {
            var song = await GetOwnSongAsync(id);
            if (song == null)
            {
                return NotFound();
            }
            LoadAlbums(song.Album!.AppUserId!.Value, song.AlbumId);
            var model = new SongViewModel
            {
                SongId = song.SongId,
                SongName = song.SongName,
                SongUrl = song.SongUrl,
                AlbumId = song.AlbumId
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSong(SongViewModel model)
        {
            var song = await GetOwnSongAsync(model.SongId);
            if (song == null)
            {
                return NotFound();
            }
            var userId = song.Album!.AppUserId!.Value;

            if (!IsOwnAlbum(model.AlbumId, userId))
            {
                ModelState.AddModelError("AlbumId", "Lütfen kendi albümlerinizden birini seçiniz.");
            }
            if (string.IsNullOrWhiteSpace(model.SongName))
            {
                ModelState.AddModelError("SongName", "Şarkı adı boş bırakılamaz.");
            }
            if (model.SongFile != null && Path.GetExtension(model.SongFile.FileName).ToLower() != ".mp3")
            {
                ModelState.AddModelError("SongFile", "Sadece mp3 dosyaları kabul edilir.");
            }
            if (ModelState.ErrorCount > 0)
            {
                model.SongUrl = song.SongUrl;
                LoadAlbums(userId, model.AlbumId);
                return View(model);
            }

            song.SongName = model.SongName;
            song.AlbumId = model.AlbumId;
            if (model.SongFile != null)
            {
                song.SongUrl = await SaveSongFileAsync(model.SongFile);
            }
            _songService.TUpdate(song);
            return RedirectToAction("Index");
        }

        // Şarkı giriş yapan sanatçının albümüne ait değilse null döner
        private async Task<Song?> GetOwnSongAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return null;
            }
            return _songService.TGetSongsWithAlbumByUserId(user.Id).FirstOrDefault(x => x.SongId == id);
        }

        private bool IsOwnAlbum(int albumId, int userId)
        {
            return _albumService.TGetAlbumsByArtist(userId).Any(x => x.AlbumId == albumId);
        }

        private void LoadAlbums(int userId, int? selectedId)
        {
            var albumList = _albumService.TGetAlbumsByArtist(userId);
            ViewBag.Albums = (from x in albumList
                              select new SelectListItem
                              {
                                  Text = x.AlbumName,
                                  Value = x.AlbumId.ToString(),
                                  Selected = x.AlbumId == selectedId
                              }).ToList();
        }

        private async Task<string> SaveSongFileAsync(IFormFile file)
        {
            var folder = Path.Combine(_environment.WebRootPath, "songs");
            Directory.CreateDirectory(folder); // klasör yoksa oluşturur
            var fileName = Guid.NewGuid() + ".mp3";
            using (var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return "/songs/" + fileName;
        }
    }
}
