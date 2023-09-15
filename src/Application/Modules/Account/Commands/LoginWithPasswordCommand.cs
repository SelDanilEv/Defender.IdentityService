using AutoMapper;
using Defender.Common.DTOs;
using Defender.Common.Errors;
using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Application.Models.LoginResponse;
using FluentValidation;
using MediatR;

namespace Defender.IdentityService.Application.Modules.Account.Commands;

public record LoginWithPasswordCommand : IRequest<LoginResponse>
{
    public string? Login { get; set; }
    public string? Password { get; set; }
};

public sealed class LoginWithPasswordCommandValidator : AbstractValidator<LoginWithPasswordCommand>
{
    public LoginWithPasswordCommandValidator()
    {
        RuleFor(s => s.Login)
                  .NotEmpty().WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_ACC_EmptyLogin));

        RuleFor(p => p.Password)
                  .NotEmpty().WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_ACC_EmptyPassword))
                  .MinimumLength(ValidationConstants.MinPasswordLength).WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_ACC_MinPasswordLength))
                  .MaximumLength(ValidationConstants.MaxPasswordLength).WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.VL_ACC_MaxPasswordLength));
    }
}

public sealed class LoginWithPasswordCommandHandler : IRequestHandler<LoginWithPasswordCommand, LoginResponse>
{
    private readonly IUserManagementService _userManagementService;
    private readonly IAccountManagementService _accountManagementService;
    private readonly ITokenManagementService _tokenManagementService;
    private readonly IMapper _mapper;

    public LoginWithPasswordCommandHandler(
        IUserManagementService userManagementService,
        IAccountManagementService accountManagementService,
        ITokenManagementService tokenManagementService,
        IMapper mapper
        )
    {
        _userManagementService = userManagementService;
        _accountManagementService = accountManagementService;
        _tokenManagementService = tokenManagementService;
        _mapper = mapper;
    }

    public async Task<LoginResponse> Handle(LoginWithPasswordCommand request, CancellationToken cancellationToken)
    {
        var response = new LoginResponse();

        response.UserInfo = await _userManagementService.GetUsersByLoginAsync(request.Login);

        var accountInfo = await _accountManagementService.GetAccountWithPasswordAsync(response.UserInfo.Id, request.Password);

        response.AccountInfo = _mapper.Map<AccountDto>(accountInfo);

        response.Token = _tokenManagementService.GenerateNewJWT(accountInfo);

        return response;
    }
}
