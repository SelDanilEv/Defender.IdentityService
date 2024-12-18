﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using Defender.Common.Attributes;
using Defender.Common.Consts;
using Defender.IdentityService.Application.Modules.AccessCode.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1;

public class AccessCodeController : BaseApiController
{
    public AccessCodeController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }

    [HttpPost("send/email")]
    [Auth(Roles.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task SendAccessCodeAsync(
        [FromBody] SendVerificationCodeCommand command)
    {
        await ProcessApiCallAsync(command);
    }

    [HttpPost("send/email/reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<Guid> SendPasswordResetAccessCodeAsync(
        [FromBody] SendPasswordResetCodeCommand command)
    {
        return await ProcessApiCallAsync<SendPasswordResetCodeCommand, Guid>(command);
    }

    [HttpPost("verify")]
    [Auth(Roles.User)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<bool> VerifyAccessCodeAsync([FromBody] VerifyCodeCommand command)
    {
        return await ProcessApiCallAsync<VerifyCodeCommand, bool>(command);
    }

}
