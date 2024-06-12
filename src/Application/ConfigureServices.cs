using System.Reflection;
using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Application.Services;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Defender.IdentityService.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.RegisterServices();

        return services;
    }

    private static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<INotificationService, NotificationService>();
        services.AddTransient<IAccessCodeService, AccessCodeService>();
        services.AddTransient<IAccountManagementService, AccountManagementService>();
        services.AddTransient<ITokenManagementService, TokenManagementService>();
        services.AddTransient<IGoogleTokenParsingService, GoogleTokenParsingService>();
        services.AddTransient<ILoginHistoryService, LoginHistoryService>();
        services.AddTransient<IUserManagementService, UserManagementService>();

        return services;
    }
}
