using AutoMapper;
using Defender.Common.Errors;
using Defender.Common.Interfaces;
using Defender.IdentityService.Application.Common.Exceptions;
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
    private readonly IAccountAccessor _accountAccessor;
    private readonly IAccountManagementService _accountManagementService;

    public ChangeUserPasswordCommandHandler(
        IAccountAccessor accountAccessor,
        IAccountManagementService accountManagementService
        )
    {
        _accountAccessor = accountAccessor;
        _accountManagementService = accountManagementService;
    }

    public async Task<Unit> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountManagementService.GetOrCreateAccountAsync(request.AccountId);

        var currentUser = _accountAccessor.AccountInfo;

        if (currentUser.IsSuperAdmin)
        {
            await _accountManagementService.ChangePasswordAsync(request.AccountId, request.NewPassword);
        }
        else if (currentUser.IsAdmin)
        {
            if (userAccount.Id == currentUser.Id || !userAccount.IsAdmin)
            {
                await _accountManagementService.ChangePasswordAsync(request.AccountId, request.NewPassword);
            }
            else
            {
                throw new ForbiddenAccessException(ErrorCode.BR_ACC_AdminCannotChangeAdminPassword);
            }
        }
        else
        {
            if (userAccount.Id == currentUser.Id)
            {
                await _accountManagementService.ChangePasswordAsync(request.AccountId, request.NewPassword);
            }
            else
            {
                throw new ForbiddenAccessException(ErrorCode.BR_ACC_UserCanUpdateOnlyOwnAccount);
            }
        }

        return Unit.Value;
    }
}
