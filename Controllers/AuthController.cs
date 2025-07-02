using DotnetAuth.Domain.Contracts;
using DotnetAuth.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAuth.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
         private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequst request)
        {
            if (request == null)
            {
                return BadRequest("Invalid registration request.");
            }
            var result = await _userService.RegisterUserAsync(request);
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid login request.");
            }
            var result = await _userService.LoginAsync(request);
            if (result == null)
            {
                return Unauthorized("Invalid username or password.");
            }
            return Ok(result);
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

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshokenRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid refresh token request.");
            }
            var result = await _userService.RefreshTokenAsync(request);
            if (result == null)
            {
                return Unauthorized("Invalid refresh token.");
            }
            return Ok(result);
        }

        [HttpPost("revoke-refresh-token")]
        [Authorize]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshokenRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid revoke refresh token request.");
            }
            var result = await _userService.RevokeRefreshTokenAsync(request);
            if (result == null)
            {
                return NotFound("Refresh token not found.");
            }
            return Ok(result);
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

        [HttpPost("update")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid login request.");
            }
            var result = await _userService.UpdateUserAsync(id,request);
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
    }
}
