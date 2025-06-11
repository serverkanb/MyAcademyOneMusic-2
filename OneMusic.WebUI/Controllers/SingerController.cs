using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OneMsuic.BusinessLayer.Abstract;
using OneMusic.DataAccessLayer.Context;
using OneMusic.EntityLayer.Entities;
using OneMusic.EntityLayer.Entity;

namespace OneMusic.WebUI.Controllers
{
    [AllowAnonymous]
    public class SingerController : Controller
    {
        private readonly ISingerService _singerService;
        private readonly IAlbumService _albumService;
        private readonly ICategoryService _categoryService;
        private readonly OneMusicContext _oneMusicContext;

        public SingerController(ISingerService singerService, IAlbumService albumService, OneMusicContext oneMusicContext, ICategoryService categoryService)
        {
            _singerService = singerService;
            _albumService = albumService;
            _oneMusicContext = oneMusicContext;
            _categoryService = categoryService;
        }

        public IActionResult Index(string search)
        {
            var singers = _singerService.TGetList();

            if (!string.IsNullOrEmpty(search))
            {
                singers=singers.Where(x=>x.Name.ToLower().Contains(search.ToLower())).ToList();
            }
          
            return View("Index",singers);
        }

        public IActionResult SingerDetails(int id)
        {
            var singer = _oneMusicContext.Singers.Include(x=>x.AppUser).Include(x=>x.Songs).ThenInclude(x=>x.Album).FirstOrDefault(x => x.SingerId == id);

            if (singer == null)
            {
                return NotFound();
            }
            return View(singer);


        }
    }
}
