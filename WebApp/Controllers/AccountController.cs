﻿using System;
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
                if (!ModelState.IsValid) //blad logowania - nie bledne haslo jako niezgodne z walidacja

                    return View(loginVM);

                //zwraca name usera do zalogowania
                var user = await _userManager.FindByNameAsync(loginVM.Username);
                if (user != null)

                {
                    //logowanie

                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Niepoprawna nazwa użytkownika lub hasło...");
                return View(loginVM);
            }

            [HttpGet]
            public async Task<IActionResult> Register()
            {
                return await Task.Run(() => { return View(new RegisterMV()); });
            }

            [HttpGet]
            public async Task<IActionResult> Login()
            {
                return await Task.Run(() => { return View(new LoginMV()); });
            }
            [HttpPost]
            public async Task<IActionResult> Register(RegisterMV r)

            {
                if (ModelState.IsValid) //wprowadzone wartości logowania zgodne z walidacją; ModelState-model predefiniowany
                {
                    var user = new User() { UserName = r.Username, Email=r.Email };
                    user.UserDetails = new UserDetails();
                    user.UserDetails.Birthday = r.Birthday;
                var result = await _userManager.CreateAsync(user, r.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home"); //(metoda, controller)
                    }
                }


                return View(r);
            }

            [HttpPost]
            public async Task<IActionResult> Logout()
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
        }
    
}