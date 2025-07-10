using ECommerce.API.Service;
using ECommerce.BLL.Implementation;
using ECommerce.BLL.Interface;
using ECommerce.DAL.Repositories.IRepository;
using ECommerce.DAL.Repositories.Repository;
using ECommerce.DAL.UnitOfWorks;

public static class ServiceColletionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        #region BLL Services
        services.AddScoped<IUserService, UserServiceImpl>();
        services.AddScoped<ITokenService, TokenServiceImple>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        #endregion

        #region DAL Repositories
        services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
        services.AddScoped<IUnitOfWorks, UnitOfWorks>();
        #endregion

        return services;
    }
}
