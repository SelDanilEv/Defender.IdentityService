using Defender.Common.Errors;
using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Domain.Enum;
using FluentValidation;
using Defender.Common.Extension;
using MediatR;

namespace Defender.IdentityService.Application.Modules.Verification.Commands;

public record SendVerificationCodeCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public AccessCodeType Type { get; set; } = AccessCodeType.Default;
};

public sealed class SendVerificationCodeCommandValidator : AbstractValidator<SendVerificationCodeCommand>
{
    public SendVerificationCodeCommandValidator()
    {
        RuleFor(s => s.UserId)
            .NotEmpty()
            .WithMessage(ErrorCode.VL_ACC_EmptyUserId);
    }
}

public sealed class SendVerificationCodeCommandHandler : IRequestHandler<SendVerificationCodeCommand, Unit>
{
    private readonly IAccessCodeService _accessCodeService;
    private readonly INotificationService _notificationService;
    private readonly IUserManagementService _userManagementService;

    public SendVerificationCodeCommandHandler(
        IAccessCodeService accessCodeService,
        INotificationService notificationService,
        IUserManagementService userManagementService
        )
    {
        _accessCodeService = accessCodeService;
        _notificationService = notificationService;
        _userManagementService = userManagementService;
    }

    public async Task<Unit> Handle(SendVerificationCodeCommand request, CancellationToken cancellationToken)
    {
        var accessCode = await _accessCodeService.CreateAccessCodeAsync(request.UserId, request.Type);

        await _notificationService.SendVerificationCodeAsync(accessCode);

        return Unit.Value;
    }
}
