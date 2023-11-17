﻿using System.Net.Http.Headers;
using System.Reflection;
using Defender.Common.Clients.Notification;
using Defender.Common.Clients.UserManagement;
using Defender.Common.Helpers;
using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Application.Common.Interfaces.Repositories;
using Defender.IdentityService.Application.Common.Interfaces.Wrapper;
using Defender.IdentityService.Application.Configuration.Options;
using Defender.IdentityService.Infrastructure.Clients.Google;
using Defender.IdentityService.Infrastructure.Clients.Notification;
using Defender.IdentityService.Infrastructure.Clients.UserManagement;
using Defender.IdentityService.Infrastructure.Repositories;
using Defender.IdentityService.Infrastructure.Repositories.AccountInfos;
using Defender.IdentityService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Defender.IdentityService.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        RegisterServices(services);

        RegisterRepositories(services);

        RegisterApiClients(services, configuration);

        RegisterClientWrappers(services);

        return services;
    }

    private static void RegisterClientWrappers(IServiceCollection services)
    {
        services.AddTransient<IUserManagementWrapper, UserManagementWrapper>();
        services.AddTransient<INotificationWrapper, NotificationWrapper>();
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddTransient<IAccountVerificationService, AccountVerificationService>();
        services.AddTransient<IAccessCodeService, AccessCodeService>();
        services.AddTransient<IAccountManagementService, AccountManagementService>();
        services.AddTransient<ITokenManagementService, TokenManagementService>();
        services.AddTransient<IGoogleTokenParsingService, GoogleTokenParsingService>();
        services.AddTransient<ILoginHistoryService, LoginHistoryService>();
        services.AddTransient<IUserManagementService, UserManagementService>();
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddSingleton<IAccessCodeRepository, AccessCodeRepository>();
        services.AddSingleton<IAccountInfoRepository, AccountInfoRepository>();
        services.AddSingleton<ILoginRecordRepository, LoginRecordRepository>();
    }

    private static void RegisterApiClients(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IGoogleClient, GoogleClient>(nameof(GoogleClient), (serviceProvider, client) =>
        {
            client.BaseAddress = new Uri(serviceProvider.GetRequiredService<IOptions<GoogleOptions>>().Value.Url);
        });

        services.RegisterUserManagementAsServiceClient(
            (serviceProvider, client) =>
            {
                client.BaseAddress = new Uri(serviceProvider.GetRequiredService<IOptions<UserManagementOptions>>().Value.Url);
                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    InternalJwtHelper.GenerateInternalJWT(configuration["JwtTokenIssuer"]));
            });

        services.RegisterNotificationAsServiceClient(
            (serviceProvider, client) =>
            {
                client.BaseAddress = new Uri(serviceProvider.GetRequiredService<IOptions<NotificationOptions>>().Value.Url);
                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    InternalJwtHelper.GenerateInternalJWT(configuration["JwtTokenIssuer"]));
            });
    }

}
