using System;
using System.Threading.Tasks;
using InternetForum.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace InternetForum.WebAPI.Controllers
{
    public class ReplyController
    {
        [Microsoft.AspNetCore.Components.Route("[Controller]")]
        public class CommentController : Controller
        {
            private readonly IReplyService _replyService;
            public CommentController(IReplyService replyService)
            {
                _replyService = replyService;
            }

            [HttpPost]
            [Authorize]
            public async Task<IActionResult> AddReply([FromBody] CreateReply reply)
            {
                await _replyService.AddAsync(reply);
                return Created("Reply created", reply);
            }


            [HttpDelete("{id}")]
            [Authorize]
            public async Task<IActionResult> DeleteReply(int id)
            {
                await _replyService.DelAsync(id);
                return NoContent();
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetReply(int id)
            {
                return Json(await _replyService.GetAsync(id));
            }



            [HttpGet("/comment/post/{id}")]
            public async Task<IActionResult> BrowseAllReplies(int id)
            {
                return Json(await _replyService.BrowseAllByPostId(id));
            }

        

            [HttpPut("{id}")]
            [Authorize]
            public async Task<IActionResult> EditReply([FromBody] EditReply reply, int id)
            {
                await _replyService.UpdateAsync(reply,id);
                return NoContent();
            }

           
        }
    }
}
