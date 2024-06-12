using AutoMapper;
using Defender.Common.DTOs;
using Defender.Common.Errors;
using Defender.Common.Exceptions;
using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Application.Models.LoginResponse;
using FluentValidation;
using Defender.Common.Extension;
using MediatR;

namespace Defender.IdentityService.Application.Modules.Account.Commands;

public record LoginGoogleCommand : IRequest<LoginResponse>
{
    public string? Token { get; set; }
};

public sealed class LoginGoogleCommandValidator : AbstractValidator<LoginGoogleCommand>
{
    public LoginGoogleCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage(ErrorCode.ES_GoogleAPIIssue);
    }
}

public sealed class LoginGoogleCommandHandler(
        IUserManagementService userManagementService,
        IAccountManagementService accountManagementService,
        ITokenManagementService tokenManagementService,
        IMapper mapper
        ) : IRequestHandler<LoginGoogleCommand, LoginResponse>
{

    public async Task<LoginResponse> Handle(
        LoginGoogleCommand request,
        CancellationToken cancellationToken)
    {
        var response = new LoginResponse();

        response.UserInfo = await userManagementService
            .CreateOrGetUserByGoogleTokenAsync(request.Token);

        var accountInfo = await accountManagementService
            .GetOrCreateAccountAsync(response.UserInfo.Id);

        accountInfo = await accountManagementService
            .UpdateEmailVerificationAsync(accountInfo.Id, true);

        if (accountInfo?.IsBlocked ?? false)
        {
            throw new ServiceException(ErrorCode.BR_ACC_UserIsBlocked);
        }

        response.AccountInfo = mapper.Map<AccountDto>(accountInfo);

        response.Token = await tokenManagementService.GenerateNewJWTAsync(accountInfo);

        return response;
    }
}
