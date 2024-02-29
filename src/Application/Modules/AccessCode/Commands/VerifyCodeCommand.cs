using Defender.Common.Errors;
using Defender.Common.Interfaces;
using Defender.IdentityService.Application.Common.Interfaces;
using FluentValidation;
using MediatR;

namespace Defender.IdentityService.Application.Modules.Verification.Commands;

public record VerifyCodeCommand : IRequest<bool>
{
    public int Code { get; set; }
};

public sealed class VerifyCodeCommandValidator : AbstractValidator<VerifyCodeCommand>
{
    public VerifyCodeCommandValidator()
    {
        RuleFor(s => s.Code)
                  .NotEmpty().WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_InvalidRequest));
    }
}

public sealed class VerifyCodeCommandHandler : IRequestHandler<VerifyCodeCommand, bool>
{
    private readonly ICurrentAccountAccessor _currentAccountAccessor;
    private readonly IAccessCodeService _accessCodeService;

    public VerifyCodeCommandHandler(
        ICurrentAccountAccessor currentAccountAccessor,
        IAccessCodeService accessCodeService)
    {
        _currentAccountAccessor = currentAccountAccessor;
        _accessCodeService = accessCodeService;
    }

    public async Task<bool> Handle(VerifyCodeCommand request, CancellationToken cancellationToken)
    {
        var currentUserAccountId = _currentAccountAccessor.GetAccountId();

        return await _accessCodeService.VerifyAccessCode(currentUserAccountId, request.Code);
    }
}
