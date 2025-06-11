using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OneMusic.EntityLayer.Entities;
using OneMusic.WebUI.Models;
using System.Security;

namespace OneMusic.WebUI.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller 
        //giriş yapmak için bu controllerı oluşturduk
    {
        private readonly UserManager<AppUser> _userManager;
        // kendi bu sınıfı verdi 

        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]

        public IActionResult SignUp() // burada asenkron yapılar kullanıcaz 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterViewModel model) // asenkron olursa aawait eklememiz gerekiyor
        {
            AppUser user = new AppUser()

            {
                Email = model.Email,
                UserName = model.UserName,
                Name = model.Name,
                SurName = model.Surname,
                
            };

            if (model.Password == model.ConfirmPassword)
            {
				var result = await _userManager.CreateAsync(user, model.Password);// user Appuser türünde bekliyor identityUser beklemiyor çünkü başta appuser türünde oluşturduk parametreyi
				if (result.Succeeded)
				{
                    //await _userManager.AddToRoleAsync(user, "Visitor");
                    return RedirectToAction("Index", "Login");
				}
				foreach (var item in result.Errors) // eğer yanlış olursa if ten çıkacak ve modelstate içerisinde hataları gösteriyoruz
				{
					ModelState.AddModelError("", item.Description);//hata açıklamaları İtem.Description
				}

			}
            

            
            return View();
        }



    }

   
}
