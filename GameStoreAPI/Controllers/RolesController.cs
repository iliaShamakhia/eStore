using GameStoreBLL.Models;
using GameStoreDAL.Data.Helpers;
using GameStoreDAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = UserRoles.Admin)]
    public class RolesController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public RolesController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("get-users")]
        public async Task<ActionResult<IEnumerable<UserWithRole>>> GetUsers()
        {   var list = new List<UserWithRole>();
            var users = await _userManager.Users.ToListAsync();
            
            foreach(var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                list.Add(new UserWithRole { UserName = user.UserName, Role = roles.First() });
            }
            return Ok(list);
        }

        [HttpPut("user/{username}/add/{role}")]
        public async Task<IActionResult> AddUserToRole([FromRoute] string username, [FromRoute] string role)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRoleAsync(user, roles.First());
                await _userManager.AddToRoleAsync(user, role);
                return Ok();
            }
            else
            {
                throw new NullReferenceException("User does not exist.");
            }
        }

    }
}
