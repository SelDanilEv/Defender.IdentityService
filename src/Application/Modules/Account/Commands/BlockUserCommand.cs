using Defender.Common.Errors;
using Defender.Common.Interfaces;
using Defender.IdentityService.Application.Common.Interfaces;
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
                  .NotEmpty().WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_InvalidRequest));
    }
}

public sealed class BlockUserCommandHandler : IRequestHandler<BlockUserCommand, Unit>
{
    private readonly IAuthorizationCheckingService _authorizationCheckingService;
    private readonly IAccountManagementService _accountManagementService;

    public BlockUserCommandHandler(
        IAuthorizationCheckingService authorizationCheckingService,
        IAccountManagementService accountManagementService
        )
    {
        _authorizationCheckingService = authorizationCheckingService;
        _accountManagementService = accountManagementService;
    }

    public async Task<Unit> Handle(BlockUserCommand request, CancellationToken cancellationToken)
    {
        await _authorizationCheckingService.RunWithAuthAsync(
            request.AccountId,
            async ()=> await _accountManagementService.BlockAsync(request.AccountId, request.DoBlockUser),
            ErrorCode.BR_ACC_SuperAdminCannotBeBlocked,
            ErrorCode.BR_ACC_AdminCannotBlockAdmins
            );

        return Unit.Value;
    }
}
