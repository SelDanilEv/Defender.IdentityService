using AutoMapper;
using Defender.Common.DTOs;
using Defender.Common.Errors;
using Defender.Common.Exceptions;
using Defender.Common.Extension;
using Defender.IdentityService.Application.Common.Interfaces.Services;
using Defender.IdentityService.Application.Models.LoginResponse;
using FluentValidation;
using MediatR;

namespace Defender.IdentityService.Application.Modules.Account.Commands;

public record LoginWithPasswordCommand : IRequest<LoginResponse>
{
    public string? Login { get; set; }
    public string? Password { get; set; }
};

public sealed class LoginWithPasswordCommandValidator
    : AbstractValidator<LoginWithPasswordCommand>
{
    public LoginWithPasswordCommandValidator()
    {
        RuleFor(s => s.Login)
            .NotEmpty().WithMessage(ErrorCode.VL_ACC_EmptyLogin);

        RuleFor(p => p.Password)
            .NotEmpty()
                .WithMessage(ErrorCode.VL_ACC_EmptyPassword)
            .MinimumLength(ValidationConstants.MinPasswordLength)
                .WithMessage(ErrorCode.VL_ACC_MinPasswordLength)
            .MaximumLength(ValidationConstants.MaxPasswordLength)
                .WithMessage(ErrorCode.VL_ACC_MaxPasswordLength);
    }
}

public sealed class LoginWithPasswordCommandHandler(
        IUserManagementService userManagementService,
        IAccountManagementService accountManagementService,
        ITokenManagementService tokenManagementService,
        IMapper mapper
        ) : IRequestHandler<LoginWithPasswordCommand, LoginResponse>
{

    public async Task<LoginResponse> Handle(LoginWithPasswordCommand request, CancellationToken cancellationToken)
    {
        var response = new LoginResponse();

        response.UserInfo = await userManagementService.GetUserByLoginAsync(request.Login);

        var accountInfo = await accountManagementService.GetAccountWithPasswordAsync(response.UserInfo.Id, request.Password);

        if (accountInfo?.IsBlocked ?? false)
        {
            throw new ServiceException(ErrorCode.BR_ACC_UserIsBlocked);
        }

        if (accountInfo == null)
        {
            throw new ServiceException(ErrorCode.BR_ACC_InvalidPassword);
        }

        response.AccountInfo = mapper.Map<AccountDto>(accountInfo);

        response.Token = await tokenManagementService
            .GenerateNewJWTAsync(accountInfo);

        return response;
    }
}
