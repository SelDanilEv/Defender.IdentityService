using Defender.Common.Errors;
using Defender.Common.Exceptions;
using Defender.Common.DB.Model;
using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Application.Common.Interfaces.Repositories;
using Defender.IdentityService.Application.Common.Interfaces.Wrapper;
using Defender.IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Defender.IdentityService.Infrastructure.Services;

public class AccountVerificationService : IAccountVerificationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly INotificationWrapper _notificationWrapper;
    private readonly IUserManagementService _userManagementService;
    private readonly IAccountInfoRepository _accountInfoRepository;
    private readonly IAccessCodeService _accessCodeService;
    private readonly IConfiguration _configuration;

    public AccountVerificationService(
        IHttpContextAccessor httpContextAccessor,
        INotificationWrapper notificationWrapper,
        IUserManagementService userManagementService,
        IAccountInfoRepository accountInfoRepository,
        IAccessCodeService accessCodeService,
        IConfiguration configuration)
    {
        _accountInfoRepository = accountInfoRepository;
        _httpContextAccessor = httpContextAccessor;
        _accessCodeService = accessCodeService;
        _userManagementService = userManagementService;
        _notificationWrapper = notificationWrapper;
        _configuration = configuration;
    }

    public async Task<string> SendVerificationEmailAsync(Guid userId)
    {
        var accessCode = await _accessCodeService.CreateEmailVerificationAccessCodeAsync(userId);
        var userInfo = await _userManagementService.GetUserByIdAsync(userId);

        var verificationLink = CreateVerificationLink(accessCode.Hash, accessCode.Code);

        return await _notificationWrapper.SendEmailVerificationAsync(userInfo.Email, verificationLink);
    }

    public async Task<AccountInfo> VerifyEmailAsync(int hash, int code)
    {
        var (isVerified, userId) = await _accessCodeService.VerifyAccessCode(hash, code);

        if (isVerified)
        {
            var updateRequest = UpdateModelRequest<AccountInfo>.Init(userId)
                                    .UpdateField(a => a.IsEmailVerified, true);

            return await _accountInfoRepository.UpdateAccountInfoAsync(updateRequest);
        }
        else
        {
            throw new ServiceException(ErrorCode.BR_ACC_CodeWasNotVerified);
        }
    }

    private string CreateVerificationLink(int hash, int code) => 
        String.Format(_configuration["VerificationEmailLinkTemplate"],
                hash,
                code);
}
