using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Defender.IdentityService.Application.Models.LoginResponse;
using Defender.IdentityService.Application.Modules.Account.Commands;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Defender.Common.Attributes;
using Defender.Common.Models;

namespace Defender.IdentityService.WebUI.Controllers.V1;

public class AccountController : BaseApiController
{
    public AccountController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }

    [HttpPost("google")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<LoginResponse> LoginWithGoogleTokenAsync([FromBody] LoginGoogleCommand loginCommand)
    {
        return await ProcessApiCallWithoutMappingAsync<LoginGoogleCommand, LoginResponse>(loginCommand);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<LoginResponse> LoginWithCredentialsAsync([FromBody] LoginWithPasswordCommand loginCommand)
    {
        return await ProcessApiCallWithoutMappingAsync<LoginWithPasswordCommand, LoginResponse>(loginCommand);
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<LoginResponse> CreateWithCredentialsAsync([FromBody] CreateAccountCommand createCommand)
    {
        return await ProcessApiCallWithoutMappingAsync<CreateAccountCommand, LoginResponse>(createCommand);
    }

    [HttpPost("password/change")]
    [Auth(Roles.User)]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task ChangeUserPasswordAsync([FromBody] ChangeUserPasswordCommand changeUserPasswordCommand)
    {
        await ProcessApiCallAsync(changeUserPasswordCommand);
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
