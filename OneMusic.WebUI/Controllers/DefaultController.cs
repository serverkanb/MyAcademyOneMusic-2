using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OneMsuic.BusinessLayer.Abstract;
using OneMusic.EntityLayer.Entities;
using OneMusic.EntityLayer.Entity;

namespace OneMusic.WebUI.Controllers
{
    
    public class DefaultController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMessageService _messageService;

        public DefaultController(UserManager<AppUser> userManager, IMessageService messageService)
        {
            _userManager = userManager;
            _messageService = messageService;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {

            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);

            ViewBag.IsAdmin = roles.Contains("Admin");
            ViewBag.IsArtist = roles.Contains("Artist");
            }
            else
            {
                ViewBag.IsAdmin = false;
                ViewBag.IsArtist = false;
            }

            return View();
        }


        [HttpPost]
        public IActionResult SendMessage(Message message)
        {
            _messageService.TCreate(message);
            return NoContent();
        }
    }
}