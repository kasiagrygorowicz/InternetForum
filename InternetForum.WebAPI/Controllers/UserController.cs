using System;
using System.Threading.Tasks;
using InternetForum.Infrastructure.Command;
using InternetForum.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternetForum.WebAPI.Controllers
{
    [Route("[Controller]")]
    public class UserController : Controller
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> AddUser([FromBody] CreateUser user)
        {
            await _userService.AddAsync(user);
            return Created("User Created", user);
        }

        [HttpDelete("{id}")]
        //[Authorize]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _userService.DelAsync(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userService.GetAsync(id);
            return Json(user);
        }

        [HttpGet]
        public async Task<IActionResult> BrowseAll()
        {
            var users = await _userService.BrowseAll();
            return Json(users);
        }

        [HttpPut("{id}")]
        //[Authorize]
        public async Task<IActionResult> EditUser([FromBody] EditUser editedUser, string id)
        {
            await _userService.UpdateAsync(editedUser,id);
            return NoContent();
        }
    }
}
