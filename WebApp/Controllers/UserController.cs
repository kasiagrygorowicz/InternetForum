using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InternetForum.Core.Domain;
using InternetForum.WebApp.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
   
        public class UserController : Controller
        {
            public IConfiguration _configuration;
            private readonly UserManager<User> _userManager;
            public UserController(IConfiguration configuration, UserManager<User> userManager)
            {
                _configuration = configuration;
                _userManager = userManager;
            }


            public async Task<IActionResult> Index()
            {
                var tokenString = Utils.GenerateJSONWebToken();
                string _restpath = GetHostUrl().Content + CN();
                List<UserMV> usersList = new List<UserMV>();
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.GetAsync(_restpath))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        usersList = JsonConvert.DeserializeObject<List<UserMV>>(apiResponse);
                    }
                }

                return View(usersList);
            }

            [HttpGet]
            [Authorize]
            public async Task<IActionResult> Edit(int id)
            {
               string _restpath = GetHostUrl().Content + "userdetails";
           
            UserDetailsMV u = new UserDetailsMV();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync($"{_restpath}/{id}"))
                    {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    u = JsonConvert.DeserializeObject<UserDetailsMV>(apiResponse);
                    }
                }
                return View(u);
            }

            [HttpPost]
            [Authorize]
            public async Task<IActionResult> Edit(EditUserDetailsMV details)
            {
                string _restpath = GetHostUrl().Content + "userdetails";
                var tokenString = Utils.GenerateJSONWebToken();

            UserDetailsMV result = new UserDetailsMV();
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        string jsonString = System.Text.Json.JsonSerializer.Serialize(details);
                        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                        using (var response = await httpClient.PutAsync($"{_restpath}/{details.Id}", content))
                        {
                        
                        string apiResponse = await response.Content.ReadAsStringAsync();
                            result = JsonConvert.DeserializeObject<UserDetailsMV>(apiResponse);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return View(ex);
                }

                return RedirectToAction(nameof(Index));
            }

         

        private ContentResult GetHostUrl()
        {
            var result = _configuration["RestApiUrl:HostUrl"];
            return Content(result);
        }

        private string CN()
        {
            string cn = ControllerContext.RouteData.Values["controller"].ToString();
            return cn;
        }

    }
}
