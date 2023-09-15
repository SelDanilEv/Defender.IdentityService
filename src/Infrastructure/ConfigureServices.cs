using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Application.Common.Interfaces.Repositories;
using Defender.IdentityService.Application.Configuration.Options;
using Defender.IdentityService.Infrastructure.Clients;
using Defender.IdentityService.Infrastructure.Clients.Interfaces;
using Defender.IdentityService.Infrastructure.Repositories.AccountInfos;
using Defender.IdentityService.Infrastructure.Repositories.LoginRecords;
using Defender.IdentityService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Defender.IdentityService.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterServices(services);

        RegisterRepositories(services);

        RegisterApiClients(services);

        return services;
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddTransient<IAccountManagementService, AccountManagementService>();
        services.AddTransient<ITokenManagementService, TokenManagementService>();
        services.AddTransient<IGoogleTokenParsingService, GoogleTokenParsingService>();
        services.AddTransient<IUserManagementService, FakeUserManagementService>();
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddSingleton<IAccountInfoRepository, AccountInfoRepository>();
        services.AddSingleton<ILoginRecordRepository, LoginRecordRepository>();
    }

    private static void RegisterApiClients(IServiceCollection services)
    {
        services.AddHttpClient<IGoogleClient, GoogleClient>("GoogleClient", (serviceProvider, client) =>
        {
            client.BaseAddress = new Uri(serviceProvider.GetRequiredService<IOptions<GoogleOption>>().Value.Url);
        });
    }

}
