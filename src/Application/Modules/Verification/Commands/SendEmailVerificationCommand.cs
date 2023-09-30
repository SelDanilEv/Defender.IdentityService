using Defender.Common.Errors;
using Defender.Common.Exceptions;
using Defender.Common.Interfaces;
using Defender.IdentityService.Application.Common.Interfaces;
using FluentValidation;
using MediatR;

namespace Defender.IdentityService.Application.Modules.Verification.Commands;

public record SendEmailVerificationCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
};

public sealed class SendEmailVerificationCommandValidator : AbstractValidator<SendEmailVerificationCommand>
{
    public SendEmailVerificationCommandValidator()
    {
        RuleFor(s => s.UserId)
                  .NotEmpty().WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_ACC_EmptyUserId));
    }
}

public sealed class SendEmailVerificationCommandHandler : IRequestHandler<SendEmailVerificationCommand, Unit>
{
    private readonly IAccountAccessor _accountAccessor;
    private readonly IAccountVerificationService _accountVerificationService;

    public SendEmailVerificationCommandHandler(
        IAccountAccessor accountAccessor,
        IAccountVerificationService accountVerificationService
        )
    {
        _accountAccessor = accountAccessor;
        _accountVerificationService = accountVerificationService;
    }

    public async Task<Unit> Handle(SendEmailVerificationCommand request, CancellationToken cancellationToken)
    {
        await _accountVerificationService.SendVerificationEmailAsync(request.UserId);

        return Unit.Value;
    }
}
