using Microsoft.AspNetCore.Authorization;
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
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _environment;

        public ProfileController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
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
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
            model.ImageUrl = user.ImageUrl; // hata durumunda mevcut resim görünmeye devam etsin

            // Mevcut şifre doğru değilse hiçbir değişiklik yapılmaz
            if (string.IsNullOrEmpty(model.OldPassword) || !await _userManager.CheckPasswordAsync(user, model.OldPassword))
            {
                ModelState.AddModelError("", "Mevcut Şifreniz Hatalı");
                return View(model);
            }

            if (model.ImageFile != null)
            {
                var extension = Path.GetExtension(model.ImageFile.FileName).ToLower();//uzantı
                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                {
                    ModelState.AddModelError("ImageFile", "Sadece Resim dosyaları kabul edilir.");
                    return View(model);
                }
                var folder = Path.Combine(_environment.WebRootPath, "Images");
                Directory.CreateDirectory(folder); // klasör yoksa oluşturur
                var imageName = Guid.NewGuid() + extension;
                using (var stream = new FileStream(Path.Combine(folder, imageName), FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }
                user.ImageUrl = "/Images/" + imageName;
            }

            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (model.NewPassword != model.ConfirmPassword)
                {
                    ModelState.AddModelError("", "Yeni şifreler uyuşmuyor");
                    return View(model);
                }
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var item in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View(model);
                }
            }

            user.Name = model.Name;
            user.SurName = model.Surname;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.UserName;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var item in updateResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(model);
            }

            // Bilgiler değiştiği için oturumu kapatıp yeniden giriş istiyoruz
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Login", new { area = "" });
        }
    }
}
