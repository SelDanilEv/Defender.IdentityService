using AutoMapper;
using Defender.Common.DTOs;
using Defender.Common.Errors;
using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Application.Models.LoginResponse;
using FluentValidation;
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
            .NotEmpty().WithMessage(ErrorCodeHelper.GetErrorCode(ErrorCode.ES_GoogleAPIIssue));
    }
}

public sealed class LoginGoogleCommandHandler : IRequestHandler<LoginGoogleCommand, LoginResponse>
{
    private readonly IUserManagementService _userManagementService;
    private readonly IAccountManagementService _accountManagementService;
    private readonly ITokenManagementService _tokenManagementService;
    private readonly IMapper _mapper;

    public LoginGoogleCommandHandler(
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

    public async Task<LoginResponse> Handle(LoginGoogleCommand request, CancellationToken cancellationToken)
    {
        var response = new LoginResponse();

        response.UserInfo = await _userManagementService.CreateOrGetUserByGoogleTokenAsync(request.Token);

        var accountInfo = await _accountManagementService.GetOrCreateAccountAsync(response.UserInfo.Id);

        response.AccountInfo = _mapper.Map<AccountDto>(accountInfo);

        response.Token = await _tokenManagementService.GenerateNewJWTAsync(accountInfo);

        return response;
    }
}
