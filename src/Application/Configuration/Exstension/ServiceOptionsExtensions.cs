using Defender.Common.Configuration.Exstension;
using Defender.IdentityService.Application.Configuration.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Defender.IdentityService.Application.Configuration.Exstension;

public static class ServiceOptionsExtensions
{
    public static IServiceCollection AddApplicationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCommonOptions(configuration);

        services.Configure<GoogleOptions>(configuration.GetSection(nameof(GoogleOptions)));

        services.Configure<UserManagementOptions>(configuration.GetSection(nameof(UserManagementOptions)));

        return services;
    }
}