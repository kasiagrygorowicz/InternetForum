using System;
using System.Threading.Tasks;
using InternetForum.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace InternetForum.WebAPI.Controllers
{
  
        [Microsoft.AspNetCore.Mvc.Route("[Controller]")]
        public class ReplyController : Controller
        {
            private readonly IReplyService _replyService;
            public ReplyController(IReplyService replyService)
            {
                _replyService = replyService;
            }

            [HttpPost]
            //[Authorize]
            public async Task<IActionResult> AddReply([FromBody] CreateReply reply)
            {
            Console.Write(reply.Author);
            Console.Write(reply.Post);
            Console.Write(reply.Content);
            await _replyService.AddAsync(reply);
                return Created("", reply);
            }


            [HttpDelete("{id}")]
            //[Authorize]
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



            [HttpGet("/reply/post/{id}")]
            public async Task<IActionResult> BrowseAllReplies(int id)
            {
                return Json(await _replyService.BrowseAllByPostId(id));
            }

        

            [HttpPut("{id}")]
            //[Authorize]
            public async Task<IActionResult> EditReply([FromBody] EditReply reply, int id)
            {
                await _replyService.UpdateAsync(reply,id);
                return NoContent();
            }

           
        }
    
}
