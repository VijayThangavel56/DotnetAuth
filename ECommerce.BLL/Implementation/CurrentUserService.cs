using ECommerce.API.Service;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ECommerce.BLL.Implementation
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

        public string? GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId;
        }
    }
}
