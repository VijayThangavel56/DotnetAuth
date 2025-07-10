using ECommerce.Domain.Entities;

namespace ECommerce.BLL.Interface
{
    public interface ITokenService
    {
        Task<string> GenerateToken(ApplicationUser user);
        string GenerateRefreshToken();
    }
}
