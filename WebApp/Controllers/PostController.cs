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
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace WebApp.Controllers
{
    public class PostController : Controller
    {
        public IConfiguration Configuration;
        private readonly UserManager<User> _userManager;

        public PostController(IConfiguration configuration, UserManager<User> userManager)
        {
            Configuration = configuration;
            _userManager = userManager;
        }

       
        public async Task<IActionResult> Index()
        {
            string _restpath = GetHostUrl().Content + CN();
            var tokenString = Utils.GenerateJSONWebToken();

            List<PostMV> postsList = new List<PostMV>();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                using (var response = await httpClient.GetAsync(_restpath))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    postsList = JsonConvert.DeserializeObject<List<PostMV>>(apiResponse);
                }
            }

            return View(postsList);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            string _restpath = GetHostUrl().Content + CN();

            PostMV post = new PostMV();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{_restpath}/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    post = JsonConvert.DeserializeObject<PostMV>(apiResponse);
                }
            }
            return View(post);
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Edit(EditPostMV post)
        {
            string _restpath = GetHostUrl().Content + CN();
            var tokenString = Utils.GenerateJSONWebToken();

            PostMV r = new PostMV();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(post);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.PutAsync($"{_restpath}/{post.Id}", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        r = JsonConvert.DeserializeObject<PostMV>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                return View(ex);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            CreatePostMV s = new CreatePostMV();
            return await Task.Run(() => View(s));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreatePostMV s)
        {
            string _restpath = GetHostUrl().Content + CN();
            var tokenString = Utils.GenerateJSONWebToken();

            s.Author = _userManager.GetUserAsync(HttpContext.User).Result.Id;
           

            PostMV result = new PostMV();
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
                        result = JsonConvert.DeserializeObject<PostMV>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                return View(ex);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            string _restpath = GetHostUrl().Content + CN();
            var tokenString = Utils.GenerateJSONWebToken();
         
            PostMV result = new PostMV();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.DeleteAsync($"{_restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<PostMV>(apiResponse);
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
