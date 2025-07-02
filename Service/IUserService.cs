using DotnetAuth.Domain.Contracts;

namespace DotnetAuth.Service
{
    public interface IUserService
    {
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
