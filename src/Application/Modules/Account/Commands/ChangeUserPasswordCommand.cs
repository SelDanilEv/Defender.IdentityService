using Defender.Common.Enums;
using Defender.Common.Errors;
using Defender.Common.Exceptions;
using Defender.Common.Extension;
using Defender.Common.Interfaces;
using Defender.IdentityService.Application.Common.Interfaces.Services;
using Defender.IdentityService.Domain.Enum;
using FluentValidation;
using MediatR;

namespace Defender.IdentityService.Application.Modules.Account.Commands;

public record ChangeUserPasswordCommand : IRequest<Unit>
{
    public Guid? AccountId { get; set; }
    public string? NewPassword { get; set; }
    public int? Code { get; set; }
};

public sealed class ChangeUserPasswordCommandValidator
    : AbstractValidator<ChangeUserPasswordCommand>
{
    public ChangeUserPasswordCommandValidator()
    {
        RuleFor(p => p.NewPassword)
          .NotEmpty()
          .WithMessage(ErrorCode.VL_ACC_EmptyPassword)
          .MinimumLength(ValidationConstants.MinPasswordLength)
          .WithMessage(ErrorCode.VL_ACC_MinPasswordLength)
          .MaximumLength(ValidationConstants.MaxPasswordLength)
          .WithMessage(ErrorCode.VL_ACC_MaxPasswordLength);
    }
}

public sealed class ChangeUserPasswordCommandHandler(
        ICurrentAccountAccessor currentAccountAccessor,
        IAccessCodeService accessCodeService,
        IAccountManagementService accountManagementService,
        IAuthorizationCheckingService authorizationCheckingService
        ) : IRequestHandler<ChangeUserPasswordCommand, Unit>
{
    public async Task<Unit> Handle(
        ChangeUserPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var accountId = request.AccountId ?? currentAccountAccessor.GetAccountId();

        if (currentAccountAccessor.HasRole(Role.Admin))
        {
            await authorizationCheckingService.ExecuteWithAuthCheckAsync(accountId,
                async () =>
                    await accountManagementService.ChangePasswordAsync(
                        accountId,
                        request.NewPassword));

            return Unit.Value;
        }

        if (!request.Code.HasValue)
            throw new ServiceException(ErrorCode.VL_ACC_EmptyAccessCode);

        var isCodeValid = await accessCodeService.VerifyAccessCode(
            accountId,
            request.Code.Value,
            AccessCodeType.ResetPassword);

        if (!isCodeValid)
            throw new ServiceException(ErrorCode.BR_ACC_InvalidAccessCode);

        await accountManagementService.ChangePasswordAsync(
            accountId,
            request.NewPassword);

        return Unit.Value;
    }
}
