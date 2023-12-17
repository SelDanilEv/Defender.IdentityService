using Defender.Common.Errors;
using Defender.Common.Exceptions;
using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Application.Common.Interfaces.Wrapper;
using Microsoft.Extensions.Configuration;

namespace Defender.IdentityService.Infrastructure.Services;

public class AccountVerificationService : IAccountVerificationService
{
    private readonly INotificationWrapper _notificationWrapper;
    private readonly IUserManagementService _userManagementService;
    private readonly IAccountManagementService _accountManagementService;
    private readonly IAccessCodeService _accessCodeService;
    private readonly IConfiguration _configuration;

    public AccountVerificationService(
        INotificationWrapper notificationWrapper,
        IUserManagementService userManagementService,
        IAccountManagementService accountManagementService,
        IAccessCodeService accessCodeService,
        IConfiguration configuration)
    {
        _accountManagementService = accountManagementService;
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

        return await _notificationWrapper
            .SendEmailVerificationAsync(userInfo.Email, verificationLink);
    }

    public async Task VerifyEmailAsync(int hash, int code)
    {
        var (isVerified, userId) = await _accessCodeService.VerifyAccessCode(hash, code);

        if (isVerified)
        {
            await _accountManagementService
                .UpdateEmailVerificationAsync(userId, true);
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
