using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OneMusic.EntityLayer.Entities;
using OneMusic.WebUI.Areas.Artist.Models;

namespace OneMusic.WebUI.Areas.Artist.Controllers
{
    [Area("Artist")]
    [Authorize(Roles = "Artist")]
    [Route("[area]/[controller]/[action]/{id?}")]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public ProfileController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public async  Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var model = new ArtistEditViewModel()
            {
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Surname = user.SurName,
                ImageUrl = user.ImageUrl,
                UserName = user.UserName
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(ArtistEditViewModel model)
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);

            if (model.ImageFile != null)
            {
                var resource = Directory.GetCurrentDirectory();//kaynak
                var extension = Path.GetExtension(model.ImageFile.FileName);//uzantı
                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                {
                    // Desteklenmeyen dosya uzantısı hatası
                    ModelState.AddModelError("ImageFile", "Sadece Resim dosyaları kabul edilir.");
                    // Gerekirse, işlemi sonlandırabilirsiniz.
                    return View(model);
                }
                var ImageName = Guid.NewGuid() + extension;
                var savelocation = resource + "/wwwroot/Images/" + ImageName;
                var stream = new FileStream(savelocation, FileMode.Create);
                await model.ImageFile.CopyToAsync(stream);
                user.ImageUrl = "/Images/" + ImageName;
            }

            user.Name = model.Name;
            user.SurName=model.Surname;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.UserName;


            var result = await _userManager.CheckPasswordAsync(user, model.OldPassword);
            if(result == true)
            {
                if (model.NewPassword != null && model.ConfirmPassword == model.NewPassword)
                {
                    var changePasswordResult = await _userManager.ChangePasswordAsync(user,model.OldPassword,model.NewPassword);
                    if (!changePasswordResult.Succeeded)
                    {
                        foreach (var item in changePasswordResult.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                            return View();
                        }
                    }
                }

                var updateResult = await _userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    return RedirectToAction("Index", "Login");
                }
            }

            ModelState.AddModelError("", "Mevcut Şifreniz Hatalı");
            return View();
        }
    }
}
