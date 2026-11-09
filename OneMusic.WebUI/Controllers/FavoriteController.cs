using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OneMsuic.BusinessLayer.Abstract;
using OneMusic.EntityLayer.Entities;

namespace OneMusic.WebUI.Controllers
{
    // Favori Müziklerim: giriş yapan her kullanıcı kullanabilir
    [Authorize]
    public class FavoriteController : Controller
    {
        private readonly IFavoriteSongService _favoriteSongService;
        private readonly ISongService _songService;
        private readonly UserManager<AppUser> _userManager;

        public FavoriteController(IFavoriteSongService favoriteSongService, ISongService songService, UserManager<AppUser> userManager)
        {
            _favoriteSongService = favoriteSongService;
            _songService = songService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var values = _favoriteSongService.TGetFavoritesByUser(user.Id);
            return View(values);
        }

        // Kalbe tıklanınca fetch ile çağrılır, şarkıyı favoriye ekler veya çıkarır
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Toggle(int songId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            if (_songService.TGetById(songId) == null)
            {
                return NotFound();
            }
            var isFavorite = _favoriteSongService.TToggleFavorite(user.Id, songId);
            return Json(new { isFavorite });
        }
    }
}
