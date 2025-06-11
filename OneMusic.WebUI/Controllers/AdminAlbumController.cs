using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OneMsuic.BusinessLayer.Abstract;
using OneMusic.DataAccessLayer.Context;
using OneMusic.EntityLayer.Entities;
using OneMusic.EntityLayer.Entity;

namespace OneMusic.WebUI.Controllers
{

    [Authorize(Roles = "Admin,Artist")]
    public class AdminAlbumController : Controller

    {
        private readonly IAlbumService _albumService;
        private readonly ISingerService _singerService;
        private readonly ICategoryService _categoryService;
        private readonly OneMusicContext _oneMusicContext;



        public AdminAlbumController(IAlbumService albumService, ISingerService singerService, ICategoryService categoryService, OneMusicContext oneMusicContext)
        {
            _albumService = albumService;
            _singerService = singerService;
            _categoryService = categoryService;
            _oneMusicContext = oneMusicContext;
        }

        public IActionResult Index()
        {

            var values = _oneMusicContext.Albums.Include(x=>x.Singer).Include(x=>x.Category).ToList();
            return View(values);
        }

        public IActionResult DeleteAlbum(int id)
        {
            _albumService.TDelete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CreateAlbum()
        {
            var singerList = _singerService.TGetList();
            ViewBag.singers = new SelectList(singerList, "SingerId", "Name");
            var categoryList = _categoryService.TGetList();
            ViewBag.category = new SelectList(categoryList, "CategoryId", "Name");

            return View();
        }

        [HttpPost]
        public IActionResult CreateAlbum(Album album)
        {
            if (ModelState.IsValid)
            {
                _albumService.TCreate(album);
                return RedirectToAction("Index");
            }

            var singerList = _singerService.TGetList();
            ViewBag.singers = new SelectList(singerList, "SingerId", "Name");

            var categoryList = _categoryService.TGetList();
            ViewBag.categories = new SelectList(categoryList, "CategoryId", "CategoryName");

            return View(album);
        }

        [HttpGet]
        public IActionResult UpdateAlbum(int id)
        {
            var album = _albumService.TGetById(id);

            var singerList = _singerService.TGetList();
            ViewBag.singers = new SelectList(singerList, "SingerId", "Name", album.SingerId);

            var categoryList = _categoryService.TGetList();
            ViewBag.categories = new SelectList(categoryList, "CategoryId", "CategoryName", album.CategoryId);

            return View(album);

        }

        [HttpPost]
        public IActionResult UpdateAlbum(Album album)
        {
            if (!ModelState.IsValid)
            {
                // Form geçersizse dropdown verilerini yeniden yüklemesi için yazdık
                var singerList = _singerService.TGetList();
                ViewBag.singers = new SelectList(singerList, "SingerId", "Name", album.SingerId);

                var categoryList = _categoryService.TGetList();
                ViewBag.categories = new SelectList(categoryList, "CategoryId", "CategoryName", album.CategoryId);

                return View(album);
            }

            _albumService.TUpdate(album);
            return RedirectToAction("Index");
        }

    }
}
