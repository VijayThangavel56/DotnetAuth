using ECommerce.BLL.Interface;
using ECommerce.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController : BaseController
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
        [AllowAnonymous]

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
    }
}
