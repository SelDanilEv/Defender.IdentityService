using System;
using System.Net.Http;
using System.Text;
using Defender.Common.Accessors;
using Defender.Common.Errors;
using Defender.Common.Exceptions;
using Defender.Common.Helpers;
using Defender.Common.Interfaces;
using Defender.IdentityService.Application.Common.Exceptions;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Defender.IdentityService.WebUI;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
    {
        services.AddSingleton<IAccountAccessor, AccountAccessor>();

        services.AddHttpContextAccessor();

        services.AddProblemDetails(options => ConfigureProblemDetails(options, environment));

        services.AddJwtAuthentication(configuration);

        services.AddSwagger();

        services.AddFluentValidationAutoValidation();

        services.AddControllers();

        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        return services;
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(auth =>
        {
            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidIssuer = configuration["JwtTokenIssuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        SecretsHelper.GetSecret(Common.Enums.Secret.JwtSecret)))
            };
        });

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Identity service",
                Description = "Service to manage generate jwt token and manage accounts authorization info",
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter your token in the text input below.\r\n\r\nExample: \"1safsfsdfdfd\"",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        return services;
    }

    private static void ConfigureProblemDetails(ProblemDetailsOptions options, IWebHostEnvironment environment)
    {
        options.IncludeExceptionDetails = (ctx, ex) => environment.IsEnvironment("Development");

        options.Map<ValidationException>(exception =>
        {
            var validationProblemDetails = new ValidationProblemDetails(exception.Errors);
            validationProblemDetails.Detail = exception.Message;
            validationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity;
            return validationProblemDetails;
        });

        options.Map<ForbiddenAccessException>(exception =>
        {
            var problemDetails = new ProblemDetails();
            problemDetails.Detail = exception.Message;
            problemDetails.Status = StatusCodes.Status403Forbidden;
            return problemDetails;
        });

        options.Map<ServiceException>(exception =>
        {
            var problemDetails = new ProblemDetails();
            problemDetails.Detail = exception.Message;
            problemDetails.Status = StatusCodes.Status400BadRequest;
            return problemDetails;
        });

        options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);

        options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);

        options.Map<Exception>(exception =>
        {
            var problemDetails = new ProblemDetails();
            problemDetails.Detail = ErrorCodeHelper.GetErrorCode(ErrorCode.UnhandledError);
            problemDetails.Status = StatusCodes.Status500InternalServerError;
            return problemDetails;
        }); ;
    }

}
