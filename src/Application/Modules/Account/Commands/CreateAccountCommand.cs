﻿using AutoMapper;
using Defender.Common.DTOs;
using Defender.Common.Errors;
using Defender.Common.Exstension;
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
                    .WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_ACC_EmptyEmail))
                  .EmailAddress()
                    .WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_ACC_InvalidEmail));

        RuleFor(p => p.Password)
                  .NotEmpty()
                    .WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_ACC_EmptyPassword))
                  .MinimumLength(ValidationConstants.MinPasswordLength)
                    .WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_ACC_MinPasswordLength))
                  .MaximumLength(ValidationConstants.MaxPasswordLength)
                    .WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_ACC_MaxPasswordLength));

        //RuleFor(p => p.PhoneNumber)
        //          .Matches(ValidationConstants.PhoneNumberRegex).WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_ACC_InvalidPhoneNumber));

        RuleFor(x => x.Nickname)
                  .NotEmpty()
                    .WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_ACC_EmptyNickname))
                  .MinimumLength(ValidationConstants.MinNicknameLength)
                    .WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_ACC_MinNicknameLength))
                  .MaximumLength(ValidationConstants.MaxNicknameLength)
                    .WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_ACC_MaxNicknameLength));
    }
}

public sealed class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, LoginResponse>
{
    private readonly IUserManagementService _userManagementService;
    private readonly IAccountManagementService _accountManagementService;
    private readonly ITokenManagementService _tokenManagementService;
    private readonly INotificationService _notificationService;
    private readonly IAccessCodeService _accessCodeService;
    private readonly IMapper _mapper;

    public CreateAccountCommandHandler(
        IUserManagementService userManagementService,
        IAccountManagementService accountManagementService,
        INotificationService notificationService,
        IAccessCodeService accessCodeService,
        ITokenManagementService tokenManagementService,
        IMapper mapper
        )
    {
        _userManagementService = userManagementService;
        _accountManagementService = accountManagementService;
        _notificationService = notificationService;
        _accessCodeService = accessCodeService;
        _tokenManagementService = tokenManagementService;
        _mapper = mapper;
    }

    public async Task<LoginResponse> Handle(
        CreateAccountCommand request, 
        CancellationToken cancellationToken)
    {
        var response = new LoginResponse();

        request.PhoneNumber = request.PhoneNumber.DigitsOnly();

        response.UserInfo = await _userManagementService.CreateUserAsync(
            request.Email,
            request.PhoneNumber, 
            request.Nickname);

        var accountInfo = await _accountManagementService.GetOrCreateAccountAsync(
            response.UserInfo.Id, 
            request.Password);

        var accessCode = await _accessCodeService.CreateAccessCodeAsync(
            response.UserInfo.Id, 
            AccessCodeType.EmailVerification);

        await _notificationService.SendVerificationCodeAsync(accessCode);

        response.AccountInfo = _mapper.Map<AccountDto>(accountInfo);

        response.Token = await _tokenManagementService.GenerateNewJWTAsync(accountInfo);

        return response;
    }
}
