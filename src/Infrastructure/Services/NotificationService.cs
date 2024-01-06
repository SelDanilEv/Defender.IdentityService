using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Application.Common.Interfaces.Wrapper;
using Defender.IdentityService.Domain.Entities;
using Defender.IdentityService.Domain.Enum;

namespace Defender.IdentityService.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationWrapper _notificationWrapper;
    private readonly IUserManagementService _userManagementService;

    public NotificationService(
        INotificationWrapper notificationWrapper,
        IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
        _notificationWrapper = notificationWrapper;
    }

    public async Task<string> SendVerificationCodeAsync(AccessCode accessCode)
    {
        var userInfo = await _userManagementService.GetUserByIdAsync(accessCode.UserId);

        switch (accessCode.Type)
        {
            case AccessCodeType.EmailVerification:
                return await _notificationWrapper
                    .SendEmailVerificationAsync(userInfo.Email, accessCode.Hash, accessCode.Code);
            case AccessCodeType.Universal:
            case AccessCodeType.UpdateAccount:
            default:
                return await _notificationWrapper
                    .SendVerificationCodeAsync(userInfo.Email, accessCode.Code);
        };
    }


}
