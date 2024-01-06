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
    private readonly IAccountAccessor _accountAccessor;
    private readonly IAccessCodeService _accessCodeService;

    public VerifyCodeCommandHandler(
        IAccountAccessor accountAccessor,
        IAccessCodeService accessCodeService
        )
    {
        _accountAccessor = accountAccessor;
        _accessCodeService = accessCodeService;
    }

    public async Task<bool> Handle(VerifyCodeCommand request, CancellationToken cancellationToken)
    {
        return await _accessCodeService.VerifyAccessCode(_accountAccessor.AccountInfo.Id, request.Code);
    }
}
