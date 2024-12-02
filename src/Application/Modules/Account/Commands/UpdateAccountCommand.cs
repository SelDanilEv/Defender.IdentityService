using Defender.Common.Enums;
using Defender.Common.Errors;
using Defender.Common.Extension;
using Defender.Common.Interfaces;
using Defender.IdentityService.Application.Common.Interfaces.Services;
using Defender.IdentityService.Application.Models.ApiRequests;
using Defender.IdentityService.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Defender.IdentityService.Application.Modules.Account.Commands;

public record UpdateAccountCommand(
        Guid Id,
        Role? Role,
        bool? IsPhoneVerified,
        bool? IsEmailVerified,
        bool? IsBlocked)
    : UpdateAccountInfoRequest(
        Id,
        Role,
        IsPhoneVerified,
        IsEmailVerified,
        IsBlocked),
    IRequest<AccountInfo>
{
};

public sealed class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator()
    {
        RuleFor(s => s.Id)
            .NotEmpty()
            .WithMessage(ErrorCode.VL_ACC_EmptyUserId);

        RuleFor(p => p)
            .Must(p => p.IsPhoneVerified.HasValue
                || p.IsEmailVerified.HasValue
                || p.Role.HasValue
                || p.IsBlocked.HasValue)
            .WithMessage(ErrorCode.VL_InvalidRequest);
    }
}

public sealed class UpdateAccountCommandHandler(
        IAuthorizationCheckingService authorizationCheckingService,
        IAccountManagementService accountManagementService)
    : IRequestHandler<UpdateAccountCommand, AccountInfo>
{
    public async Task<AccountInfo> Handle(
        UpdateAccountCommand request,
        CancellationToken cancellationToken)
    {
        var account = await authorizationCheckingService.ExecuteWithAuthCheckAsync(
            request.Id,
            async () => await accountManagementService.UpdateAccountInfoAsync(request));

        return account;
    }
}
