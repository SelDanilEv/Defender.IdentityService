using AutoMapper;
using Defender.Common.DTOs;
using Defender.Common.Errors;
using Defender.Common.Extension;
using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Application.Models.LoginResponse;
using Defender.IdentityService.Domain.Enum;
using FluentValidation;
using MediatR;

namespace Defender.IdentityService.Application.Modules.Account.Commands;

public record CreateAccountCommand : IRequest<LoginResponse>
{
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Nickname { get; set; }
    public string? Password { get; set; }
};

public sealed class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(s => s.Email)
                  .NotEmpty()
                    .WithMessage(ErrorCode.VL_ACC_EmptyEmail)
                  .EmailAddress()
                    .WithMessage(ErrorCode.VL_ACC_InvalidEmail);

        RuleFor(p => p.Password)
                  .NotEmpty()
                    .WithMessage(ErrorCode.VL_ACC_EmptyPassword)
                  .MinimumLength(ValidationConstants.MinPasswordLength)
                    .WithMessage(ErrorCode.VL_ACC_MinPasswordLength)
                  .MaximumLength(ValidationConstants.MaxPasswordLength)
                    .WithMessage(ErrorCode.VL_ACC_MaxPasswordLength);

        //RuleFor(p => p.PhoneNumber)
        //          .Matches(ValidationConstants.PhoneNumberRegex).WithMessage(ErrorCode.VL_ACC_InvalidPhoneNumber));

        RuleFor(x => x.Nickname)
                  .NotEmpty()
                    .WithMessage(ErrorCode.VL_ACC_EmptyNickname)
                  .MinimumLength(ValidationConstants.MinNicknameLength)
                    .WithMessage(ErrorCode.VL_ACC_MinNicknameLength)
                  .MaximumLength(ValidationConstants.MaxNicknameLength)
                    .WithMessage(ErrorCode.VL_ACC_MaxNicknameLength);
    }
}

public sealed class CreateAccountCommandHandler(
        IUserManagementService userManagementService,
        IAccountManagementService accountManagementService,
        INotificationService notificationService,
        IAccessCodeService accessCodeService,
        ITokenManagementService tokenManagementService,
        IMapper mapper
        ) : IRequestHandler<CreateAccountCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(
        CreateAccountCommand request,
        CancellationToken cancellationToken)
    {
        var response = new LoginResponse();

        request.PhoneNumber = request.PhoneNumber.DigitsOnly();

        response.UserInfo = await userManagementService.CreateUserAsync(
            request.Email,
            request.PhoneNumber,
            request.Nickname);

        var accountInfo = await accountManagementService.GetOrCreateAccountAsync(
            response.UserInfo.Id,
            request.Password);

        var accessCode = await accessCodeService.CreateAccessCodeAsync(
            response.UserInfo.Id,
            AccessCodeType.EmailVerification);

        await notificationService.SendVerificationCodeAsync(accessCode);

        response.AccountInfo = mapper.Map<AccountDto>(accountInfo);

        response.Token = await tokenManagementService.GenerateNewJWTAsync(accountInfo);

        return response;
    }
}
