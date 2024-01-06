using Defender.Common.Errors;
using Defender.IdentityService.Application.Common.Interfaces;
using FluentValidation;
using MediatR;

namespace Defender.IdentityService.Application.Modules.Account.Commands;

public record ChangeUserPasswordCommand : IRequest<Unit>
{
    public Guid AccountId { get; set; }
    public string? NewPassword { get; set; }
};

public sealed class ChangeUserPasswordCommandValidator : AbstractValidator<ChangeUserPasswordCommand>
{
    public ChangeUserPasswordCommandValidator()
    {
        RuleFor(s => s.AccountId)
                  .NotEmpty().WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_InvalidRequest));

        RuleFor(p => p.NewPassword)
          .NotEmpty().WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_ACC_EmptyPassword))
          .MinimumLength(ValidationConstants.MinPasswordLength).WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_ACC_MinPasswordLength))
          .MaximumLength(ValidationConstants.MaxPasswordLength).WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_ACC_MaxPasswordLength));
    }
}

public sealed class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, Unit>
{
    private readonly IAccountManagementService _accountManagementService;

    public ChangeUserPasswordCommandHandler(
        IAccountManagementService accountManagementService
        )
    {
        _accountManagementService = accountManagementService;
    }

    public async Task<Unit> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
    {
        await _accountManagementService.ChangePasswordAsync(request.AccountId, request.NewPassword);

        return Unit.Value;
    }
}
