﻿using Defender.Common.Errors;
using Defender.Common.Extension;
using Defender.Common.Interfaces;
using Defender.IdentityService.Application.Common.Interfaces.Services;
using FluentValidation;
using MediatR;

namespace Defender.IdentityService.Application.Modules.Account.Commands;

public record BlockUserCommand : IRequest<Unit>
{
    public Guid AccountId { get; set; }
    public bool DoBlockUser { get; set; } = true;
};

public sealed class BlockUserCommandValidator : AbstractValidator<BlockUserCommand>
{
    public BlockUserCommandValidator()
    {
        RuleFor(s => s.AccountId)
            .NotEmpty()
            .WithMessage(ErrorCode.VL_InvalidRequest);
    }
}

public sealed class BlockUserCommandHandler(
        IAuthorizationCheckingService authorizationCheckingService,
        IAccountManagementService accountManagementService
        ) : IRequestHandler<BlockUserCommand, Unit>
{

    public async Task<Unit> Handle(BlockUserCommand request, CancellationToken cancellationToken)
    {
        await authorizationCheckingService.ExecuteWithAuthCheckAsync(
            request.AccountId,
            async () => await accountManagementService.BlockAsync(request.AccountId, request.DoBlockUser),
            false,
            ErrorCode.BR_ACC_AdminCannotBlockAdmins
            );

        return Unit.Value;
    }
}
