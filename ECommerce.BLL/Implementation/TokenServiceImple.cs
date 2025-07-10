using ECommerce.BLL.Interface;
using ECommerce.Domain.Entities;
using ECommerce.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.BLL.Implementation
{
    public class TokenServiceImple : ITokenService
    {
        private readonly SymmetricSecurityKey _secretKey;
        private readonly string? _validIssuer;
        private readonly string? _validAudience;
        private readonly double _expires;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<TokenServiceImple> _logger;

        public TokenServiceImple(
             IConfiguration configuration,
             UserManager<ApplicationUser> userManager,
             ILogger<TokenServiceImple> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));


            var jwtSetting = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            if (jwtSetting == null)
            {
                throw new ArgumentNullException(nameof(jwtSetting), "JwtSettings configuration section is missing or invalid.");
            }

            _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key));
            _validIssuer = jwtSetting.ValidIssuer;
            _validAudience = jwtSetting.ValidAudience;
            _expires = Convert.ToDouble(jwtSetting.Expires);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<string> GenerateToken(ApplicationUser user)
        {
            var signingCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);
            var claims = await GetClaimsAsyc(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private async Task<List<Claim>> GetClaimsAsyc(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Fix: Ensure 'Id' is a string or convert it to string
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim("FirstName", user.FirstName ?? string.Empty),
                new Claim("LastName", user.LastName ?? string.Empty),
                new Claim("Gender", user.Gender ?? string.Empty),
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            return new JwtSecurityToken(
                issuer: _validIssuer,
                audience: _validAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expires),
                signingCredentials: signingCredentials
            );
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var refreshToken = Convert.ToBase64String(randomNumber);
            return refreshToken;
        }


    }
}
