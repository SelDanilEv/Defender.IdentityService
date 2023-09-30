using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Defender.IdentityService.Application.Models.LoginResponse;
using Defender.IdentityService.Application.Modules.Account.Commands;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Defender.Common.Attributes;
using Defender.Common.Models;

namespace Defender.IdentityService.WebApi.Controllers.V1;

public class AccessCodeController : BaseApiController
{
    public AccessCodeController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }

    [HttpPost("block")]
    [Auth(Roles.Admin)]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task BlockUserAsync([FromBody] BlockUserCommand blockUserCommand)
    {
        await ProcessApiCallAsync(blockUserCommand);
    }

}
