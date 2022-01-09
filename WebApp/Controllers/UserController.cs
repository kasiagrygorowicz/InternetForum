using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InternetForum.Core.Domain;
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
            public IConfiguration Configuration;
            private readonly UserManager<User> _userManager;
            public UserController(IConfiguration configuration, UserManager<User> userManager)
            {
                Configuration = configuration;
                _userManager = userManager;
            }


            public async Task<IActionResult> Index()
            {
                string _restpath = GetHostUrl().Content + CN();
                var tokenString = GenerateJSONWebToken();

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
                string _restpath = GetHostUrl().Content + "UserDetails";

                UserDetailsMV s = new UserDetailsMV();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync($"{_restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        s = JsonConvert.DeserializeObject<UserDetailsMV>(apiResponse);
                    }
                }
                return View(s);
            }

            [HttpPost]
            [Authorize]
            public async Task<IActionResult> Edit(EditUserDetailsMV u)
            {
                string _restpath = GetHostUrl().Content + "userinfo";
                var tokenString = GenerateJSONWebToken();

            EditUserDetailsMV result = new EditUserDetailsMV();
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        string jsonString = System.Text.Json.JsonSerializer.Serialize(u);
                        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                        using (var response = await httpClient.PutAsync($"{_restpath}/{u.Id}", content))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            result = JsonConvert.DeserializeObject<EditUserDetailsMV>(apiResponse);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return View(ex);
                }

                return RedirectToAction(nameof(Index));
            }

            private string GenerateJSONWebToken()
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperTajneHaslo111222333"));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                new Claim("Name", "Kasia"),
                new Claim(JwtRegisteredClaimNames.Email, "01153047@pw.edu.pl")
            };

                var token = new JwtSecurityToken(
                    issuer: "https://localhost:5001/",
                    audience: "https://localhost:44300/",
                    expires: DateTime.Now.AddHours(2),
                    signingCredentials: credentials,
                    claims: claims
                    );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

        private ContentResult GetHostUrl()
        {
            var result = Configuration["RestApiUrl:HostUrl"];
            return Content(result);
        }

        private string CN()
        {
            string cn = ControllerContext.RouteData.Values["controller"].ToString();
            return cn;
        }

    }
}
