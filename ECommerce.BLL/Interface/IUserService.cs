using ECommerce.Domain.Entities;
using ECommerce.DTO;

namespace ECommerce.BLL.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUserAsync();
        Task<bool> AssignRolesAsync(string userId, IEnumerable<string> roles);
        Task<UserResponse> RegisterUserAsync(UserRegisterRequst userRegisterRequst);
        Task<CurrentUserResponse> GetCurrentUserAsync();
        Task<UserResponse> GetByIdAsync(Guid id);
        Task<UserResponse> UpdateUserAsync(Guid id,UpdateUserRequest updateUserRequest);
        Task DeleteAsync(Guid id);
        Task<RevokeRefreshTokenResponse> RevokeRefreshTokenAsync(RefreshokenRequest refreshokenRequest);
        Task<CurrentUserResponse> RefreshTokenAsync(RefreshokenRequest refreshokenRequest);
        Task<UserResponse> LoginAsync(UserLoginRequest userLoginRequest);
    }
}
