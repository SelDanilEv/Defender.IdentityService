using Defender.Common.Extension;
using Defender.IdentityService.Application.Configuration.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Defender.IdentityService.Application.Configuration.Extension;

public static class ServiceOptionsExtensions
{
    public static IServiceCollection AddApplicationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GoogleOptions>(configuration.GetSection(nameof(GoogleOptions)));

        services.Configure<UserManagementOptions>(configuration.GetSection(nameof(UserManagementOptions)));

        services.Configure<NotificationOptions>(configuration.GetSection(nameof(NotificationOptions)));

        return services;
    }
}