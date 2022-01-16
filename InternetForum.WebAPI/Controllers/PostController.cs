using System;
using System.Threading.Tasks;
using InternetForum.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternetForum.WebAPI.Controllers
{
    [Route("[Controller]")]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPost([FromBody] CreatePost post)
        {
            await _postService.AddAsync(post);
            return Created("Post Created", post);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _postService.DelAsync(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postService.GetAsync(id);
            return Json(post);
        }

        [HttpGet]
        public async Task<IActionResult> BrowseAll()
        {
            var posts = await _postService.BrowseAll();
            return Json(posts);
        }

     

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> EditPost([FromBody] EditPost post, int id)
        {
            await _postService.UpdateAsync(post,id);
            return NoContent();
        }

        
    }
}
