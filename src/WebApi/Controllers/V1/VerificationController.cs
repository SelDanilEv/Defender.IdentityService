using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Defender.IdentityService.Application.Models.LoginResponse;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Defender.IdentityService.Application.Modules.Verification.Commands;
using Defender.Common.Attributes;
using Defender.Common.Models;

namespace Defender.IdentityService.WebApi.Controllers.V1;

public class VerificationController : BaseApiController
{
    public VerificationController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }

    [HttpPost("verify/email")]
    [Auth(Roles.User)]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task SendVerificationEmailAsync([FromBody] SendEmailVerificationCommand command)
    {
        await ProcessApiCallAsync<SendEmailVerificationCommand>(command);
    }



    [HttpGet("verify/email")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<bool> VerifyEmailAsync([FromQuery] VerifyEmailCommand command)
    {
        return await ProcessApiCallWithoutMappingAsync<VerifyEmailCommand, bool>(command);
    }

}
