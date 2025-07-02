using DotnetAuth.Domain.Contracts;
using DotnetAuth.Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DotnetAuth.Extensions
{
    public static partial class ApplicationService
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("corsPolicy",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
        }
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentityCore<IdentityUser>(o =>
            {
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = true;
                o.Password.RequireUppercase = false;
                o.Password.RequiredLength = 8;

            }).AddEntityFrameworkStores<ApplicationDbContext>().
            AddDefaultTokenProviders();

        }
        public static void ConfigureJwt(this IServiceCollection services)
        {
            var jwtSettings = services.BuildServiceProvider()
                .GetRequiredService<IConfiguration>()
                .GetSection("JwtSettings")
                .Get<JwtSettings>();

            if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Key)){
                throw new ArgumentNullException(nameof(jwtSettings), "JwtSettings configuration section is missing or invalid.");
            } 

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.ValidIssuer,
                    ValidAudience = jwtSettings.ValidAudience,
                    IssuerSigningKey = secretKey
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                         context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.HttpContext.Response.ContentType = "application/json";
                        var result= System.Text.Json.JsonSerializer.Serialize(new
                        {
                            StatusCode = StatusCodes.Status401Unauthorized,
                            Message = "Authentication failed. Please check your credentials.",
                            Title = "Unauthorized"
                        });
                        return context.Response.WriteAsync(result, context.HttpContext.RequestAborted);
                    }
                };
            });
        }
    }
}
