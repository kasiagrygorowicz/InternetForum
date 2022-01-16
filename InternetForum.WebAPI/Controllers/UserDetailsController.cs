using System;
using System.Threading.Tasks;
using InternetForum.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace InternetForum.WebAPI.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("[Controller]")]
    public class UserDetailsController : Controller
    {
        private IUserDetailsService _userDetailsService;
        public UserDetailsController(IUserDetailsService userDetailsServiceervice)
        {
            _userDetailsService = userDetailsServiceervice;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddUserDetails([FromBody] CreateUserDetails userInfo)
        {
            await _userDetailsService.AddAsync(userInfo);
            return Created("UserDetails created", userInfo);
        }


        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUserDetails(int id)
        {
            await _userDetailsService.DelAsync(id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> BrowseAll()
        {
            var usersInfo = await _userDetailsService.BrowseAll();
            return Json(usersInfo);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserDetails(int id)
        {
            var userInfo = await _userDetailsService.GetAsync(id);
            return Json(userInfo);
        }

        

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> EditUserDetails([FromBody] EditUserDetails user, int id)
        {
          
            await _userDetailsService.UpdateAsync( user,id);
            return NoContent();
        }

     
    }
}
