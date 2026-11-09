using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OneMusic.WebUI.Areas.Artist.Controllers
{
    [Area("Artist")]
    [Authorize(Roles = "Artist")]
    public class ArtistLayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
