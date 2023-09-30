﻿using Defender.Common.Errors;
using Defender.Common.Interfaces;
using Defender.IdentityService.Application.Common.Interfaces;
using FluentValidation;
using MediatR;

namespace Defender.IdentityService.Application.Modules.Verification.Commands;

public record VerifyEmailCommand : IRequest<bool>
{
    public int Hash { get; set; }
    public int Code { get; set; }
};

public sealed class VerifyEmailCommandValidator : AbstractValidator<VerifyEmailCommand>
{
    public VerifyEmailCommandValidator()
    {
        RuleFor(s => s.Hash)
                  .NotEmpty().WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_InvalidRequest));
        RuleFor(s => s.Code)
                  .NotEmpty().WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_InvalidRequest));
    }
}

public sealed class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, bool>
{
    private readonly IAccountAccessor _accountAccessor;
    private readonly IAccountVerificationService _accountVerificationService;

    public VerifyEmailCommandHandler(
        IAccountAccessor accountAccessor,
        IAccountVerificationService accountVerificationService
        )
    {
        _accountAccessor = accountAccessor;
        _accountVerificationService = accountVerificationService;
    }

    public async Task<bool> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountVerificationService.VerifyEmailAsync(request.Hash, request.Code);

        return account.IsEmailVerified;
    }
}
