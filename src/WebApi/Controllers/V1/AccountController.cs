using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Defender.IdentityService.Application.Models.LoginResponse;
using Defender.IdentityService.Application.Modules.Account.Commands;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Defender.Common.Attributes;
using Defender.Common.DTOs;
using Defender.IdentityService.Application.Modules.Account.Queries;
using Defender.Common.Consts;

namespace Defender.IdentityService.WebApi.Controllers.V1;

public class AccountController : BaseApiController
{
    public AccountController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }

    [HttpGet("details")]
    [Auth(Roles.User)]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<AccountDto> GetAccountDetailsAsync(
        [FromQuery] GetAccountDetailsQuery query)
    {
        return await ProcessApiCallAsync<GetAccountDetailsQuery, AccountDto>(query);
    }

    [HttpPost("google")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<LoginResponse> LoginWithGoogleTokenAsync(
        [FromBody] LoginGoogleCommand loginCommand)
    {
        return await ProcessApiCallWithoutMappingAsync
            <LoginGoogleCommand, LoginResponse>(loginCommand);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<LoginResponse> LoginWithCredentialsAsync(
        [FromBody] LoginWithPasswordCommand loginCommand)
    {
        return await ProcessApiCallWithoutMappingAsync
            <LoginWithPasswordCommand, LoginResponse>(loginCommand);
    }

    [Auth(Roles.SuperAdmin)]
    [HttpPost("login-as-admin")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<LoginResponse> LoginAsAdminAsync([FromBody] LoginAsAdminCommand command)
    {
        return await ProcessApiCallWithoutMappingAsync
            <LoginAsAdminCommand, LoginResponse>(command);
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<LoginResponse> CreateWithCredentialsAsync(
        [FromBody] CreateAccountCommand createCommand)
    {
        return await ProcessApiCallWithoutMappingAsync
            <CreateAccountCommand, LoginResponse>(createCommand);
    }

    [HttpPut("update")]
    [Auth(Roles.Admin)]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<AccountDto> UpdateAccountAsync(
        [FromBody] UpdateAccountCommand updateAccountCommand)
    {
        return await ProcessApiCallAsync
            <UpdateAccountCommand, AccountDto>(updateAccountCommand);
    }

    [HttpPut("password/change")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task ChangeUserPasswordAsync(
        [FromBody] ChangeUserPasswordCommand changeUserPasswordCommand)
    {
        await ProcessApiCallAsync(changeUserPasswordCommand);
    }

    [HttpPut("block")]
    [Auth(Roles.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task BlockUserAsync([FromBody] BlockUserCommand blockUserCommand)
    {
        await ProcessApiCallAsync(blockUserCommand);
    }

    [HttpPost("verify/email")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<bool> VerifyEmailAsync([FromBody] VerifyEmailCommand command)
    {
        return await ProcessApiCallWithoutMappingAsync<VerifyEmailCommand, bool>(command);
    }
}
