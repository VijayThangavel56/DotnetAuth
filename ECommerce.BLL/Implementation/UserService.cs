using AutoMapper;
using ECommerce.API.Service;
using ECommerce.BLL.Interface;
using ECommerce.Domain.Entities;
using ECommerce.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace ECommerce.BLL.Implementation
{
    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(
            ITokenService tokenService,
            ICurrentUserService currentUserService,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            ILogger<UserService> logger)
        {
            _tokenService = tokenService;
            _currentUserService = currentUserService;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<UserResponse> RegisterUserAsync(UserRegisterRequst userRegisterRequst)
        {
            _logger.LogInformation("Registering user with email: {Email}", userRegisterRequst.Email);
            var exsistingUser = await _userManager.FindByEmailAsync(userRegisterRequst.Email);
            if (exsistingUser != null)
            {
                _logger.LogWarning("User with email {Email} already exists.", userRegisterRequst.Email);
                throw new Exception("User already exists");
            }
            var newUser = _mapper.Map<ApplicationUser>(userRegisterRequst);

            newUser.UserName = GenerateUserName(userRegisterRequst.FirstName, userRegisterRequst.LastName);
            var result = await _userManager.CreateAsync(newUser, userRegisterRequst.Password);
            if (!result.Succeeded)
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Falied to create user:{errorMessage}", errorMessage);
                throw new Exception($"User creation failed: {errorMessage}");

            }
            _logger.LogInformation("User Created Successfully");
            await _tokenService.GenerateToken(newUser);
            return _mapper.Map<UserResponse>(newUser);

        }

        public async Task<UserResponse> LoginAsync(UserLoginRequest userLoginRequest)
        {
            if (userLoginRequest == null)
            {
                _logger.LogError("User login request is null.");
                throw new ArgumentNullException(nameof(userLoginRequest), "User login request cannot be null.");
            }
            var user = await _userManager.FindByEmailAsync(userLoginRequest.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, userLoginRequest.Password))
            {
                _logger.LogWarning("User with email {Email} not found.", userLoginRequest.Email);
                throw new Exception("User not found");
            }

            var token = await _tokenService.GenerateToken(user);

            var refreshToken = _tokenService.GenerateRefreshToken();

            using var sha256 = SHA256.Create();
            var hashedRefreshToken = Convert.ToBase64String(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(refreshToken)));
            user.RefreshToken = hashedRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(2); // Set refresh token expiry time


            //update user information to database

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var errorMessage = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                _logger.LogError("Failed to update user: {ErrorMessage}", errorMessage);
                throw new Exception($"User update failed: {errorMessage}");
            }
            var userResponse = _mapper.Map<ApplicationUser, UserResponse>(user);
            userResponse.AccessToken = token;
            userResponse.RefreshToken = refreshToken;
            return userResponse;
        }

        private string GenerateUserName(string firstName, string lastName)
        {
            var baseUserName = $"{firstName}{lastName}";

            var userName = baseUserName;
            var counter = 1;
            while (_userManager.Users.Any(u => u.UserName == userName))
            {
                userName = $"{baseUserName}{counter}";
                counter++;
            }
            return userName;
        }

        public async Task<UserResponse> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Retrieving user with ID: {UserId}", id);
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", id);
                throw new Exception("User not found");
            }
            _logger.LogInformation("User with ID {UserId} retrieved successfully.", id);
            return _mapper.Map<UserResponse>(user);
        }
        public async Task<CurrentUserResponse> GetCurrentUserAsync()
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.GetUserId());
            if (user == null)
            {
                _logger.LogWarning("Current user not found.");
                throw new Exception("User not found");
            }
            return _mapper.Map<CurrentUserResponse>(user);
        }


        public async Task<CurrentUserResponse> RefreshTokenAsync(RefreshokenRequest refreshokenRequest)
        {
            _logger.LogInformation("Refreshing token for user with refresh token: {RefreshToken}", refreshokenRequest.RefreshToken);
            if (refreshokenRequest == null || string.IsNullOrEmpty(refreshokenRequest.RefreshToken))
            {
                _logger.LogError("Refresh token request is null or empty.");
                throw new ArgumentNullException(nameof(refreshokenRequest), "Refresh token request cannot be null or empty.");
            }

            using var sha256 = SHA256.Create();
            var hashedRefreshToken = Convert.ToBase64String(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(refreshokenRequest.RefreshToken)));
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == hashedRefreshToken);

            if (user == null)
            {
                _logger.LogWarning("User with refresh token {RefreshToken} not found.", refreshokenRequest.RefreshToken);
                throw new Exception("Invalid refresh token");
            }

            //validate refresh token expiry time
            if (user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                _logger.LogWarning("Refresh token for user {UserId} has expired.", user.Id);
                throw new Exception("Refresh token has expired");
            }
            var newAccessToken = await _tokenService.GenerateToken(user);
            var currentUserResponse = _mapper.Map<CurrentUserResponse>(user);
            currentUserResponse.AccessToken = newAccessToken;
            _logger.LogInformation("Token refreshed successfully for user with ID: {UserId}", user.Id);
            return currentUserResponse;
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", id);
                throw new Exception("User not found");
            }
            _logger.LogInformation("Deleting user with ID: {UserId}", id);
            await _userManager.DeleteAsync(user);
        }









        public async Task<RevokeRefreshTokenResponse> RevokeRefreshTokenAsync(RefreshokenRequest refreshokenRequest)
        {
            _logger.LogInformation("Revoking refresh token");

            using var sha256 = SHA256.Create();
            var hashedRefreshToken = Convert.ToBase64String(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes
                (refreshokenRequest.RefreshToken)));
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == hashedRefreshToken);
            if (user == null)
            {
                _logger.LogWarning("User with refresh token {RefreshToken} not found.", refreshokenRequest.RefreshToken);
                throw new Exception("Invalid refresh token");
            }

            if (user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                _logger.LogWarning("Refresh token for user {UserId} has expired.", user.Id);
                throw new Exception("Refresh token has expired");
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null; // Clear the refresh token expiry time
            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                var errorMessage = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                _logger.LogError("Failed to revoke refresh token: {ErrorMessage}", errorMessage);
                return new RevokeRefreshTokenResponse
                {
                    Message = "Failed to revoke refresh token: " + errorMessage,
                };
            }

            _logger.LogInformation("Refresh token revoked successfully for user with ID: {UserId}", user.Id);
            return new RevokeRefreshTokenResponse
            {
                Message = "Refresh token revoked successfully"
            };
        }

        public async Task<UserResponse> UpdateUserAsync(Guid id, UpdateUserRequest updateUserRequest)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", id);
                throw new Exception("User not found");
            }
            user.FirstName = updateUserRequest.FirstName;
            user.LastName = updateUserRequest.LastName;
            user.Email = updateUserRequest.Email;
            user.UserName = GenerateUserName(updateUserRequest.FirstName, updateUserRequest.LastName);
            user.UpdateAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            _logger.LogInformation("User with ID {UserId} updated successfully.", id);
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUserAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<bool> AssignRolesAsync(string userId, IEnumerable<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var currentRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!result.Succeeded) return false;

            result = await _userManager.AddToRolesAsync(user, roles);
            return result.Succeeded;
        }
    }
}
