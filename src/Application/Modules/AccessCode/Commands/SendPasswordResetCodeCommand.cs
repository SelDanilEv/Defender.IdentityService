using Defender.Common.Errors;
using Defender.Common.Exceptions;
using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Domain.Enum;
using FluentValidation;
using MediatR;

namespace Defender.IdentityService.Application.Modules.Verification.Commands;

public record SendPasswordResetCodeCommand : IRequest<Guid>
{
    public string? Email { get; set; }
};

public sealed class SendPasswordResetCodeCommandValidator
    : AbstractValidator<SendPasswordResetCodeCommand>
{
    public SendPasswordResetCodeCommandValidator()
    {
        RuleFor(s => s.Email)
                  .NotEmpty().WithMessage(
            ErrorCodeHelper.GetErrorCode(ErrorCode.VL_ACC_EmptyEmail));
    }
}

public sealed class SendPasswordResetCodeCommandHandler(
        IAccessCodeService accessCodeService,
        INotificationService notificationService,
        IUserManagementService userManagementService
        )
    : IRequestHandler<SendPasswordResetCodeCommand, Guid>
{
    public async Task<Guid> Handle(
        SendPasswordResetCodeCommand request,
        CancellationToken cancellationToken)
    {
        var userId = await userManagementService.GetUserIdByEmailAsync(request.Email);

        if (userId == Guid.Empty)
        {
            throw new ServiceException(ErrorCode.CM_NotFound);
        }

        var accessCode = await accessCodeService.CreateAccessCodeAsync(userId, AccessCodeType.ResetPassword);

        await notificationService.SendVerificationCodeAsync(accessCode, request.Email);

        return userId;
    }
}
