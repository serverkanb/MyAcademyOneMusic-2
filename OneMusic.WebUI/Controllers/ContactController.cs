using Microsoft.AspNetCore.Mvc;
using OneMsuic.BusinessLayer.Abstract;

namespace OneMusic.WebUI.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        public IActionResult Index()
        {
            var values = _contactService.TGetList().OrderByDescending(x => x.ContactId).FirstOrDefault();
            return View(values);
        }
    }
}
