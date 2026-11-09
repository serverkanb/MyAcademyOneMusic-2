using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OneMsuic.BusinessLayer.Abstract;
using OneMsuic.BusinessLayer.Validators;
using OneMusic.EntityLayer.Entities;
using OneMusic.WebUI.Models;

namespace OneMusic.WebUI.Controllers
{
    // Çalma Listelerim: giriş yapan her kullanıcı kendi listelerini oluşturup yönetebilir
    [Authorize]
    public class PlaylistController : Controller
    {
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
            var playlist = new Playlist
            {
                Name = model.Name?.Trim() ?? "",
                Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim(),
                AppUserId = CurrentUserId
            };

            var validator = new PlaylistValidator();
            var result = validator.Validate(playlist);
            if (!result.IsValid)
            {
                TempData["playlistError"] = result.Errors[0].ErrorMessage;
                return RedirectToAction("Index");
            }

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
            playlist.Name = model.Name?.Trim() ?? "";
            playlist.Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim();

            var validator = new PlaylistValidator();
            var result = validator.Validate(playlist);
            if (!result.IsValid)
            {
                TempData["playlistError"] = result.Errors[0].ErrorMessage;
                return RedirectToAction("Detail", new { id = model.PlaylistId });
            }

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
            var userId = CurrentUserId;
            var playlist = new Playlist { Name = name?.Trim() ?? "", AppUserId = userId };

            var validator = new PlaylistValidator();
            var result = validator.Validate(playlist);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors[0].ErrorMessage);
            }

            _playlistService.TCreate(playlist);
            _playlistService.TToggleSong(userId, playlist.PlaylistId, songId);
            return Json(new { id = playlist.PlaylistId, name = playlist.Name });
        }
    }
}
