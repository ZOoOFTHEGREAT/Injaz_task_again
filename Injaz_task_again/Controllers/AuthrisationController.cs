using Injaz_task_again.Controllers;
using Injaz_task_again.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace injaz_task.Controllers
{
    public class AuthrisationController : Controller
    {

        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthrisationController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminLogin(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Home/Index.cshtml", model);
            }

            if (string.IsNullOrEmpty(model.Username))
            {
                ModelState.AddModelError("Username", "Username is required for admin login.");
                return View("~/Views/Home/Index.cshtml", model);
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("Password", "Password is required for admin login.");
                return View("~/Views/Home/Index.cshtml", model);
            }

            var user = await _signInManager.UserManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                ModelState.AddModelError("", "Username not found.");
                return View("~/Views/Home/Index.cshtml", model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded && await _signInManager.UserManager.IsInRoleAsync(user, "Admin"))
            {
                return Redirect("/Polls/Polls");
            }

            ModelState.AddModelError("", "Invalid password or you do not have admin privileges.");
            return View("~/Views/Home/Index.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClientLogin(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Home/Index.cshtml", model);
            }

            if (string.IsNullOrEmpty(model.Email))
            {
                ModelState.AddModelError("Email", "Email is required for client login.");
                return View("~/Views/Home/Index.cshtml", model);
            }

            var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _signInManager.UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await _signInManager.UserManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, isPersistent: model.RememberMe);
                    return Redirect("/Polls/Last");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("~/Views/Home/Index.cshtml", model);
                }
            }
            await _signInManager.SignInAsync(user, isPersistent: model.RememberMe);
            return Redirect("/Polls/Last");
        }
    }
}
