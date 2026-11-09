using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneMsuic.BusinessLayer.Abstract;

namespace OneMusic.WebUI.Controllers
{
    // Ana sayfadaki iletişim formundan gelen mesajlar
    [Authorize(Roles = "Admin")]
    public class AdminMessageController : Controller
    {
        private readonly IMessageService _messageService;

        public AdminMessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public IActionResult Index()
        {
            var values = _messageService.TGetList().OrderByDescending(x => x.MessageId).ToList();
            return View(values);
        }

        public IActionResult DeleteMessage(int id)
        {
            _messageService.TDelete(id);
            return RedirectToAction("Index");
        }
    }
}
