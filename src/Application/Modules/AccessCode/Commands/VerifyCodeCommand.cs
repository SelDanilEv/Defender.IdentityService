using Defender.Common.Errors;
using Defender.Common.Extension;
using Defender.Common.Interfaces;
using Defender.IdentityService.Application.Common.Interfaces.Services;
using Defender.IdentityService.Domain.Enum;
using FluentValidation;
using MediatR;

namespace Defender.IdentityService.Application.Modules.AccessCode.Commands;

public record VerifyCodeCommand : IRequest<bool>
{
    public int Code { get; set; }
    public AccessCodeType Type { get; set; } = AccessCodeType.Default;
};

public sealed class VerifyCodeCommandValidator : AbstractValidator<VerifyCodeCommand>
{
    public VerifyCodeCommandValidator()
    {
        RuleFor(s => s.Code)
            .NotEmpty()
            .WithMessage(ErrorCode.VL_ACC_EmptyAccessCode);
    }
}

public sealed class VerifyCodeCommandHandler(
        ICurrentAccountAccessor currentAccountAccessor,
        IAccessCodeService accessCodeService)
    : IRequestHandler<VerifyCodeCommand, bool>
{
    public async Task<bool> Handle(VerifyCodeCommand request, CancellationToken cancellationToken)
    {
        var currentUserAccountId = currentAccountAccessor.GetAccountId();

        return await accessCodeService.VerifyAccessCode(currentUserAccountId, request.Code, request.Type);
    }
}
