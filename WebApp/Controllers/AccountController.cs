using System;
using System.Threading.Tasks;
using WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using static WebApp.Models.LoginMV;
using InternetForum.Core.Domain;

namespace WebApp.Controllers
{

        public class AccountController : Controller
        {
            private readonly SignInManager<User> _signInManager;
            private readonly UserManager<User> _userManager;

            public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
            {
                _signInManager = signInManager;
                _userManager = userManager;
            }

            // GET
            [HttpPost]
            public async Task<IActionResult> Login(LoginMV loginVM)
            {
                if (!ModelState.IsValid)
            {
                return View(loginVM);
            }
                var user = await _userManager.FindByNameAsync(loginVM.Username);
                if (user != null)

                {
                    var r = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (r.Succeeded)
                    {
                        ViewBag.UserId = user.Id;
                        return RedirectToAction("Index", "Post");
                    }
                }
                ModelState.AddModelError("", "Wrong username or password");
                return View(loginVM);
            }

            [HttpGet]
            public async Task<IActionResult> Register()
            {
                return await Task.Run(() =>
                {
                    return View(new RegisterMV());
                });
            }

            [HttpGet]
            public async Task<IActionResult> Login()
            {
                return await Task.Run(() => { return View(new LoginMV()); });
            }
            [HttpPost]
            public async Task<IActionResult> Register(RegisterMV r)
            {
                if (ModelState.IsValid) 
                {
                    var user = new User() { UserName = r.Username, Email=r.Email };
                    user.UserDetails = new UserDetails();
                    user.UserDetails.Birthday = r.Birthday;
                var result = await _userManager.CreateAsync(user, r.Password);
                    if (result.Succeeded)
                    {
                    await _userManager.AddToRoleAsync(user, "user");
                    return RedirectToAction("Login", "Account"); 
                    }
                }


                return View(r);
            }

            [HttpPost]
            public async Task<IActionResult> Logout()
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Post");
            }
        }
    
}
