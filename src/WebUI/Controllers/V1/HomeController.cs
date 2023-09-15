using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Defender.IdentityService.Application.Modules.Home.Queries;
using Defender.Common.Attributes;
using Defender.Common.Models;
using Defender.Common.Enums;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Defender.IdentityService.WebUI.Controllers.V1;

public class HomeController : BaseApiController
{
    public HomeController(
        IMediator mediator,
        IMapper mapper) 
        : base(mediator, mapper)
    {
    }

    [HttpGet("health")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<object> HealthCheckAsync()
    {
        return new { Status = "Healthy" };
    }

    [HttpGet("authorization/check")]
    [Auth(Roles.User)]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<object> AuthorizationCheckAsync()
    {
        return new { IsAuthorized = true };
    }

    [Auth(Roles.SuperAdmin)]
    [HttpGet("configuration")]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<Dictionary<string, string>> GetConfigurationAsync(ConfigurationLevel configurationLevel)
    {
        var query = new GetConfigurationQuery()
        {
            Level = configurationLevel
        };

        return await ProcessApiCallWithoutMappingAsync<GetConfigurationQuery, Dictionary<string, string>>(query);
    }
}
