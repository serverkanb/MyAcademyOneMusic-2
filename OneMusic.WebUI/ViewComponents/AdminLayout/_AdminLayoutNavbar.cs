using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OneMusic.EntityLayer.Entities;

namespace OneMusic.WebUI.ViewComponents.AdminLayout
{
    public class _AdminLayoutNavbar : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public _AdminLayoutNavbar(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var name = User.Identity?.Name;
            var user = name == null ? null : await _userManager.FindByNameAsync(name);
            ViewBag.username = user == null ? "" : user.Name + " " + user.SurName;
            return View();
        }
    }
}
