using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OneMsuic.BusinessLayer.Abstract;
using OneMusic.DataAccessLayer.Context;
using OneMusic.EntityLayer.Entities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OneMusic.WebUI.Controllers
{
    [AllowAnonymous]
    public class AlbumsController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAlbumService _albumService;
        private readonly OneMusicContext _oneMusicContext;
        private readonly ISingerService _singerService;

        public AlbumsController(ICategoryService categoryService, UserManager<AppUser> userManager, IAlbumService albumService, OneMusicContext oneMusicContext, ISingerService singerService)
        {
            _categoryService = categoryService;
            _userManager = userManager;
            _albumService = albumService;
            _oneMusicContext = oneMusicContext;
            _singerService = singerService;
        }

        public IActionResult Index()
        {
            var categoryList = _categoryService.TGetList();
            var artistList = _singerService.TGetList();

            List<SelectListItem> artists = (from x in artistList
                                            select new SelectListItem
                                            {
                                                Text = x.Name,
                                                Value = x.Name
                                            }).ToList();

            ViewBag.artists = artists;

            List<SelectListItem> categories = (from x in categoryList
                                               select new SelectListItem
                                               {
                                                   Text = x.CategoryName,
                                                   Value = x.CategoryName,
                                               }).ToList();

            ViewBag.categories = categories;

            if (TempData["filterAlbums"] != null)
            {


                var filterList = TempData["filterAlbums"].ToString();

                var albums = JsonSerializer.Deserialize<List<Album>>(filterList, new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                });
                if (albums != null)
                {
                    return View(albums);
                }

            }

            var values = _albumService.TGetAlbumswithArtist();
            return View(values);
        }

        [HttpGet]
        public PartialViewResult FilterAlbums()
        {
            var categoryList = _categoryService.TGetList();
            var artistList = _singerService.TGetList();

            List<SelectListItem> artists = (from x in artistList
                                            select new SelectListItem
                                            {
                                                Text = x.Name,
                                                Value = x.Name
                                            }).ToList();

            ViewBag.artists = artists;

            List<SelectListItem> categories = (from x in categoryList
                                               select new SelectListItem
                                               {
                                                   Text = x.CategoryName,
                                                   Value = x.CategoryName,
                                               }).ToList();

            ViewBag.categories = categories;

            return PartialView();
        }

        [HttpPost]
        public IActionResult FilterAlbums(string category, string artist)
        {
            var query = _oneMusicContext.Albums
                .Include(x => x.Singer)
                .Include(x => x.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(x => x.Category.CategoryName == category);
            }

            if (!string.IsNullOrEmpty(artist))
            {
                query = query.Where(x => x.Singer.Name == artist);
            }

            var values = query.ToList();

            TempData["filterAlbums"] = JsonSerializer.Serialize(values, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult AlbumDetail(int id)
        {
            var album = _oneMusicContext.Albums.Include(x=>x.Singer).Include(x=>x.Category).Include(x=>x.Songs).FirstOrDefault(x=>x.AlbumId == id);

            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

    }

}
