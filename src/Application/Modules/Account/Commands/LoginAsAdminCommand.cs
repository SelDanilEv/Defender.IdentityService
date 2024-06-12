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

public record LoginAsAdminCommand : IRequest<LoginResponse>
{
    public Guid? UserId { get; set; }
};

public sealed class LoginAsAdminCommandValidator : AbstractValidator<LoginAsAdminCommand>
{
    public LoginAsAdminCommandValidator()
    {
        RuleFor(s => s.UserId)
                  .NotEmpty()
                  .WithMessage(
                    ErrorCodeHelper.GetErrorCode(ErrorCode.VL_ACC_EmptyUserId));
    }
}

public sealed class LoginAsAdminCommandHandler(
        IUserManagementService userManagementService,
        IAccountManagementService accountManagementService,
        ITokenManagementService tokenManagementService,
        IMapper mapper
        ) : IRequestHandler<LoginAsAdminCommand, LoginResponse>
{

    public async Task<LoginResponse> Handle(LoginAsAdminCommand request, CancellationToken cancellationToken)
    {
        var response = new LoginResponse();

        response.UserInfo = await userManagementService.GetUserByIdAsync(request.UserId.Value);

        var accountInfo = await accountManagementService.GetAccountByIdAsync(request.UserId.Value);

        if (accountInfo?.IsBlocked ?? false)
        {
            throw new ServiceException(ErrorCode.BR_ACC_UserIsBlocked);
        }

        response.AccountInfo = mapper.Map<AccountDto>(accountInfo);

        response.Token = await tokenManagementService.GenerateNewJWTAsync(accountInfo);

        return response;
    }
}
