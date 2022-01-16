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
    public class ReplyController : Controller
    {
        public IConfiguration Configuration;
        private readonly UserManager<User> _userManager;

        public ReplyController(IConfiguration configuration, UserManager<User> userManager)
        {
            Configuration = configuration;
            _userManager = userManager;
        }

   

        public async Task<IActionResult> Index(int id)
        {
            string _restpath = GetHostUrl().Content + CN();
            var tokenString = GenerateJSONWebToken();

            List<ReplyMV> commentsList = new List<ReplyMV>();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                using (var response = await httpClient.GetAsync($"{_restpath}/post/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    commentsList = JsonConvert.DeserializeObject<List<ReplyMV>>(apiResponse);
                }
            }

            ViewBag.PostId = id;

            return View(commentsList);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int post)
        {
            Console.Write("\n\npost" + post + "\n\n");
           
            string _restpath = GetHostUrl().Content + CN();

            ReplyMV s = new ReplyMV();
            s.PostId = post;
           

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{_restpath}/{post}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    s = JsonConvert.DeserializeObject<ReplyMV>(apiResponse);
                    
                }
            }
            return View(s);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditReplyMV p)
        {
          
            string _restpath = GetHostUrl().Content + CN();
            var tokenString = GenerateJSONWebToken();
            var id = 0;
          

            ReplyMV result = new ReplyMV();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(p);
                    Console.Write(jsonString);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.PutAsync($"{_restpath}/{p.Id}", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<ReplyMV>(apiResponse);
                    }

                    using (var response = await httpClient.GetAsync($"{_restpath}/{p.Id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<ReplyMV>(apiResponse);
                        id = result.PostId;
                    }
                }
            }
            catch (Exception ex)
            {
                return View(ex);
            }
          
            return RedirectToAction("Index",new { id=id });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create(int id)
        {
            CreateReplyMV s = new CreateReplyMV();
            s.Post = id;
            return await Task.Run(() => View(s));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateReplyMV s)
        {
            string _restpath = GetHostUrl().Content + CN();
            var tokenString = GenerateJSONWebToken();

            s.Author = _userManager.GetUserAsync(HttpContext.User).Result.Id;

            ReplyMV result = new ReplyMV();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(s);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.PostAsync($"{_restpath}", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<ReplyMV>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                return View(ex);
            }

            return RedirectToAction(nameof(Index),new { id = s.Post });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id,int post)
        {
           
            string _restpath = GetHostUrl().Content + CN();
            var tokenString = GenerateJSONWebToken();

            ReplyMV result = new ReplyMV();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.DeleteAsync($"{_restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<ReplyMV>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                return View(ex);
            }

            return RedirectToAction(nameof(Index), new { id=post });
        }

        private string GenerateJSONWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperTajneHaslo123123123"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Name", "Kasia"),
                new Claim(JwtRegisteredClaimNames.Email, "01153047@pw.edu.pl")
            };

            var token = new JwtSecurityToken(
                issuer: "https://localhost:5001/",
                audience: "https://localhost:50001/",
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
