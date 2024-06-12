using System.Reflection;
using Defender.Common.Clients.Notification;
using Defender.Common.Clients.UserManagement;
using Defender.IdentityService.Application.Common.Interfaces.Clients;
using Defender.IdentityService.Application.Common.Interfaces.Repositories;
using Defender.IdentityService.Application.Common.Interfaces.Wrapper;
using Defender.IdentityService.Application.Configuration.Options;
using Defender.IdentityService.Infrastructure.Clients.Google;
using Defender.IdentityService.Infrastructure.Clients.Notification;
using Defender.IdentityService.Infrastructure.Clients.UserManagement;
using Defender.IdentityService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Defender.IdentityService.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services
            .RegisterRepositories()
            .RegisterApiClients(configuration)
            .RegisterClientWrappers();

        return services;
    }

    private static IServiceCollection RegisterClientWrappers(this IServiceCollection services)
    {
        services.AddTransient<IUserManagementWrapper, UserManagementWrapper>();
        services.AddTransient<INotificationWrapper, NotificationWrapper>();

        return services;
    }

    private static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IAccessCodeRepository, AccessCodeRepository>();
        services.AddSingleton<IAccountInfoRepository, AccountInfoRepository>();
        services.AddSingleton<ILoginRecordRepository, LoginRecordRepository>();

        return services;
    }

    private static IServiceCollection RegisterApiClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IGoogleClient, GoogleClient>(nameof(GoogleClient), (serviceProvider, client) =>
        {
            client.BaseAddress = new Uri(serviceProvider.GetRequiredService<IOptions<GoogleOptions>>().Value.Url);
        });

        services.RegisterUserManagementClient(
            (serviceProvider, client) =>
            {
                client.BaseAddress = new Uri(serviceProvider.GetRequiredService<IOptions<UserManagementOptions>>().Value.Url);
            });

        services.RegisterNotificationClient(
            (serviceProvider, client) =>
            {
                client.BaseAddress = new Uri(serviceProvider.GetRequiredService<IOptions<NotificationOptions>>().Value.Url);
            });

        return services;
    }

}
