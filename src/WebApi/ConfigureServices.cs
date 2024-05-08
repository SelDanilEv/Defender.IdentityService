﻿using System;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using Defender.Common.Errors;
using Defender.Common.Exceptions;
using Defender.Common.Exstension;
using Defender.Common.Helpers;
using Defender.IdentityService.Application.Configuration.Exstension;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


namespace Defender.IdentityService.WebApi;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
    {
        services.AddCommonServices(configuration);

        services.AddApplicationOptions(configuration);

        services.AddHttpContextAccessor();

        services.AddProblemDetails(options => ConfigureProblemDetails(options, environment));

        services.AddJwtAuthentication(configuration);

        services.AddSwagger();

        services.AddFluentValidationAutoValidation();

        services.AddControllers().AddJsonOptions(x =>
        {
            x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

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
                    Encoding.UTF8.GetBytes(SecretsHelper.GetSecretAsync(Common.Enums.Secret.JwtSecret).Result))
            };
        });

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.UseInlineDefinitionsForEnums();

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Identity Service",
                Description = "Service to manage generate jwt token and manage accounts authorization info",
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter your token in the text input below.\r\n\r\nExample: \"1sample\"",
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

    private static void ConfigureProblemDetails(Hellang.Middleware.ProblemDetails.ProblemDetailsOptions options, IWebHostEnvironment environment)
    {
        options.IncludeExceptionDetails = (ctx, ex) => environment.IsLocalOrDevelopment();

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
        });
    }

}
