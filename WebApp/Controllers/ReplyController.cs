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
    public class ReplyController : Controller
    {
        public IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public ReplyController(IConfiguration configuration, UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int id)
        {
            var tokenString = Utils.GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();
            var x = GetHostUrl().Content;
            PostMV tmp;

            List<ReplyMV> replies = new List<ReplyMV>();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                using (var response = await httpClient.GetAsync($"{_restpath}/post/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    replies = JsonConvert.DeserializeObject<List<ReplyMV>>(apiResponse);
                }

                if (x != null)
                {
                    using (var response = await httpClient.GetAsync($"{x}post/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        tmp = JsonConvert.DeserializeObject<PostMV>(apiResponse);



                        if (tmp != null)
                        {


                            ViewBag.PostId = id;
                            ViewBag.User = tmp.AuthorNickname;
                            ViewBag.Description = tmp.Description;
                            ViewBag.Date = tmp.Posted;
                            ViewBag.Title = tmp.Title;
                        }

                    }
                }

            }

           


            return View(replies);
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id,int post)
        {
            string _restpath = GetHostUrl().Content + CN();
            var tokenString = Utils.GenerateJSONWebToken();
            ReplyMV r = new ReplyMV();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.DeleteAsync($"{_restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        r = JsonConvert.DeserializeObject<ReplyMV>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                return View(ex);
            }

            return RedirectToAction(nameof(Index), new { id=post });
        }
        
        
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create(int id)
        {
            CreateReplyMV s = new CreateReplyMV();
            s.Post = id;
            ViewBag.PostId = id;
            return await Task.Run(() => View(s));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateReplyMV s)
        {
            string _restpath = GetHostUrl().Content + CN();
            var tokenString = Utils.GenerateJSONWebToken();
            s.Author = _userManager.GetUserAsync(HttpContext.User).Result.Id;
            ReplyMV r = new ReplyMV();
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
                        r = JsonConvert.DeserializeObject<ReplyMV>(apiResponse);
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
        public async Task<IActionResult> Edit(int id)
        {
           
            Console.Write("\n\n"+id+"\n\n");
            string _restpath = GetHostUrl().Content + CN();

            ReplyMV s = new ReplyMV();
            //s.PostId = post;
           

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{_restpath}/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    s = JsonConvert.DeserializeObject<ReplyMV>(apiResponse);
                    ViewBag.PostId = s.PostId;
                }
            }
           
            return View(s);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditReplyMV p)
        {

            string _restpath = GetHostUrl().Content + CN();
            var tokenString = Utils.GenerateJSONWebToken();
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

            return RedirectToAction("Index", new { id = id });
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
