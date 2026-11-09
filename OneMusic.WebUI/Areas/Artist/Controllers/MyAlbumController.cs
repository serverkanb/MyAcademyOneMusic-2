using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OneMsuic.BusinessLayer.Abstract;
using OneMusic.EntityLayer.Entities;

namespace OneMusic.WebUI.Areas.Artist.Controllers
{
    //.net 8 ile gelen özellik contructor oluşturma şekli daha fazlasını eklemek için _albumservice yanına virgül koyup devam edebiliyoruz.
    [Area("Artist")]
    [Authorize(Roles = "Artist")]
    [Route("[area]/[controller]/[action]/{id?}")]
    public class MyAlbumController(IAlbumService _albumService, UserManager<AppUser> _userManager, ISingerService _singerService, ICategoryService _categoryService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
            var values = _albumService.TGetAlbumsByArtist(user.Id);
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateAlbum()
        {
            LoadCategories(null);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAlbum(Album album)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
            if (!ModelState.IsValid)
            {
                LoadCategories(album.CategoryId);
                return View(album);
            }
            album.AppUserId = user.Id;
            // Sanatçı kullanıcısına bağlı bir Singer kaydı varsa albümü ona bağlıyoruz
            album.SingerId = _singerService.TGetList().FirstOrDefault(x => x.AppUserId == user.Id)?.SingerId;
            _albumService.TCreate(album);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteAlbum(int id)
        {
            var album = await GetOwnAlbumAsync(id);
            if (album == null)
            {
                return NotFound();
            }
            _albumService.TDelete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAlbum(int id)
        {
            var album = await GetOwnAlbumAsync(id);
            if (album == null)
            {
                return NotFound();
            }
            LoadCategories(album.CategoryId);
            return View(album);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAlbum(Album album)
        {
            var existing = await GetOwnAlbumAsync(album.AlbumId);
            if (existing == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                LoadCategories(album.CategoryId);
                return View(album);
            }
            // Sadece formdan gelen alanları güncelliyoruz, sahiplik bilgisi değişmiyor
            existing.AlbumName = album.AlbumName;
            existing.CoverImage = album.CoverImage;
            existing.Price = album.Price;
            existing.CategoryId = album.CategoryId;
            _albumService.TUpdate(existing);
            return RedirectToAction("Index");
        }

        // Albüm giriş yapan sanatçıya ait değilse null döner (kontrol iş katmanında)
        private async Task<Album?> GetOwnAlbumAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            return user == null ? null : _albumService.TGetArtistAlbum(user.Id, id);
        }

        private void LoadCategories(int? selectedId)
        {
            ViewBag.categories = new SelectList(_categoryService.TGetList(), "CategoryId", "CategoryName", selectedId);
        }
    }
}
