using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OneMusic.EntityLayer.Entities;

namespace OneMusic.WebUI.Controllers
{

    public class AdminLayoutContrroller : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public AdminLayoutContrroller(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.username = user.Name + "" + user.SurName;

            return View();
        }

        
    }
}
