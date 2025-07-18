using ECommerce.BLL.Interface;
using ECommerce.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet("user/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            return Ok(user);
        }
        [HttpPost("update")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid login request.");
            }
            var result = await _userService.UpdateUserAsync(id, request);
            return Ok(result);
        }

        [HttpDelete("delete")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid ID.");
            }

            await _userService.DeleteAsync(id);

            return Ok($"User with ID {id} has been deleted.");
        }

        [HttpPost("assign-roles")]
        [Authorize]
        public async Task<IActionResult> AssignRoles(string userId, [FromBody] List<string> roles)
        {
            if (string.IsNullOrEmpty(userId) || roles == null || roles.Count == 0)
            {
                return BadRequest("Invalid user ID or roles.");
            }
            var result = await _userService.AssignRolesAsync(userId, roles);
            if (result)
            {
                return Ok("Roles assigned successfully.");
            }
            return BadRequest("Failed to assign roles.");
        }

        [HttpGet("get-all-users")]
        [Authorize]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _userService.GetAllUserAsync();
            if (users == null || !users.Any())
            {
                return NotFound("No users found.");
            }
            return Ok(users);
        }

        [HttpGet("current-user")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userService.GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound("Current user not found.");
            }
            return Ok(user);
        }
    }
}
