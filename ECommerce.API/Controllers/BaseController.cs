using ECommerce.DTO;
using ECommerce.Shared.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Authorize]
    [ProducesErrorResponseType(typeof(ApiResponseDto))]
    public class BaseController : ControllerBase
    {
        protected virtual bool IsAuthenticated
        {
            get
            {
                return RequestParser.IsAuthenticated(HttpContext);
            }
        }

        protected virtual LoggedUser LoggedUser
        {
            get
            {
                if (RequestParser.IsAuthenticated(HttpContext))
                {
                    var user = RequestParser.LoggedUser(HttpContext);
                    if (user != null)
                    {
                        return user;
                    }
                }

                throw new UnauthorizedAccessException("Unauthorized access.");
            }
        }
        protected IActionResult ApiResponseResult(ApiResponseDto result)
        {
            if (result.Success)
            {
                if (result.Result == null)
                {
                    return NoContent();
                }

                return StatusCode((int)result.StatusCode, result.Result);
            }
            else
            {
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    result.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                }

                return StatusCode((int)result.StatusCode, result);
            }
        }
    }
}
