using Defender.Common.Errors;
using Defender.Common.Extension;
using Defender.IdentityService.Application.Common.Interfaces;
using FluentValidation;
using MediatR;

namespace Defender.IdentityService.Application.Modules.Account.Commands;

public record VerifyEmailCommand : IRequest<bool>
{
    public int Hash { get; set; }
    public int Code { get; set; }
};

public sealed class VerifyEmailCommandValidator : AbstractValidator<VerifyEmailCommand>
{
    public VerifyEmailCommandValidator()
    {
        RuleFor(s => s.Hash)
            .NotEmpty()
            .WithMessage(ErrorCode.VL_InvalidRequest);

        RuleFor(s => s.Code)
            .NotEmpty()
            .WithMessage(ErrorCode.VL_InvalidRequest);
    }
}

public sealed class VerifyEmailCommandHandler(
        IAccountManagementService accountManagementService
        ) : IRequestHandler<VerifyEmailCommand, bool>
{

    public async Task<bool> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        await accountManagementService.VerifyEmailAsync(request.Hash, request.Code);

        return true;
    }
}
