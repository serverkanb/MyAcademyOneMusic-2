using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OneMsuic.BusinessLayer.Abstract;
using OneMusic.EntityLayer.Entities;
using OneMusic.WebUI.Models;

namespace OneMusic.WebUI.Controllers
{
    // Çalma Listelerim: giriş yapan her kullanıcı kendi listelerini oluşturup yönetebilir
    [Authorize]
    public class PlaylistController : Controller
    {
        private const int NameMaxLength = 100;
        private const int DescriptionMaxLength = 500;

        private readonly IPlaylistService _playlistService;
        private readonly ISongService _songService;
        private readonly UserManager<AppUser> _userManager;

        public PlaylistController(IPlaylistService playlistService, ISongService songService, UserManager<AppUser> userManager)
        {
            _playlistService = playlistService;
            _songService = songService;
            _userManager = userManager;
        }

        private int CurrentUserId => int.Parse(_userManager.GetUserId(User)!);

        // Kullanıcının tüm listeleri + yeni liste formu
        public IActionResult Index()
        {
            var values = _playlistService.TGetPlaylistsByUser(CurrentUserId);
            return View(values);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PlaylistViewModel model)
        {
            var error = Validate(model);
            if (error != null)
            {
                TempData["playlistError"] = error;
                return RedirectToAction("Index");
            }

            var playlist = new Playlist
            {
                Name = model.Name!.Trim(),
                Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim(),
                AppUserId = CurrentUserId
            };
            _playlistService.TCreate(playlist);
            return RedirectToAction("Detail", new { id = playlist.PlaylistId });
        }

        public IActionResult Detail(int id)
        {
            var playlist = _playlistService.TGetUserPlaylistWithSongs(CurrentUserId, id);
            if (playlist == null)
            {
                return NotFound();
            }
            // Şarkılar eklenme sırasına göre
            playlist.PlaylistSongs = playlist.PlaylistSongs?.OrderBy(x => x.AddedDate).ThenBy(x => x.PlaylistSongId).ToList();
            return View(playlist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(PlaylistViewModel model)
        {
            var playlist = _playlistService.TGetUserPlaylist(CurrentUserId, model.PlaylistId);
            if (playlist == null)
            {
                return NotFound();
            }
            var error = Validate(model);
            if (error != null)
            {
                TempData["playlistError"] = error;
                return RedirectToAction("Detail", new { id = model.PlaylistId });
            }
            playlist.Name = model.Name!.Trim();
            playlist.Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim();
            _playlistService.TUpdate(playlist);
            return RedirectToAction("Detail", new { id = playlist.PlaylistId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var playlist = _playlistService.TGetUserPlaylist(CurrentUserId, id);
            if (playlist == null)
            {
                return NotFound();
            }
            _playlistService.TDelete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveSong(int playlistId, int songId)
        {
            if (!_playlistService.TRemoveSong(CurrentUserId, playlistId, songId))
            {
                return NotFound();
            }
            return RedirectToAction("Detail", new { id = playlistId });
        }

        // ----- Şarkıların yanındaki "+" butonu için JSON uç noktaları -----

        // Kullanıcının listeleri ve şarkının hangi listelerde olduğu
        [HttpGet]
        public IActionResult ForSong(int songId)
        {
            var userId = CurrentUserId;
            var containing = _playlistService.TGetPlaylistIdsContainingSong(userId, songId).ToHashSet();
            var playlists = _playlistService.TGetPlaylistsByUser(userId)
                .Select(x => new
                {
                    id = x.PlaylistId,
                    name = x.Name,
                    songCount = x.PlaylistSongs?.Count ?? 0,
                    containsSong = containing.Contains(x.PlaylistId)
                });
            return Json(playlists);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleSong(int playlistId, int songId)
        {
            if (_songService.TGetById(songId) == null)
            {
                return NotFound();
            }
            var result = _playlistService.TToggleSong(CurrentUserId, playlistId, songId);
            if (result == null)
            {
                return NotFound();
            }
            return Json(new { inPlaylist = result.Value });
        }

        // Yeni liste oluşturur ve şarkıyı hemen içine ekler
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateWithSong(string? name, int songId)
        {
            if (_songService.TGetById(songId) == null)
            {
                return NotFound();
            }
            var error = Validate(new PlaylistViewModel { Name = name });
            if (error != null)
            {
                return BadRequest(error);
            }
            var userId = CurrentUserId;
            var playlist = new Playlist { Name = name!.Trim(), AppUserId = userId };
            _playlistService.TCreate(playlist);
            _playlistService.TToggleSong(userId, playlist.PlaylistId, songId);
            return Json(new { id = playlist.PlaylistId, name = playlist.Name });
        }

        private static string? Validate(PlaylistViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return "Çalma listesi adı boş bırakılamaz.";
            }
            if (model.Name.Trim().Length > NameMaxLength)
            {
                return $"Çalma listesi adı en fazla {NameMaxLength} karakter olabilir.";
            }
            if (model.Description != null && model.Description.Trim().Length > DescriptionMaxLength)
            {
                return $"Açıklama en fazla {DescriptionMaxLength} karakter olabilir.";
            }
            return null;
        }
    }
}
