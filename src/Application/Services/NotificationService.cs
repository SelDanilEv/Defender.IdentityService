using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Application.Common.Interfaces.Wrapper;
using Defender.IdentityService.Domain.Entities;
using Defender.IdentityService.Domain.Enum;

namespace Defender.IdentityService.Application.Services;

public class NotificationService(
    INotificationWrapper notificationWrapper,
    IUserManagementService userManagementService) : INotificationService
{
    public async Task<string> SendVerificationCodeAsync(AccessCode accessCode, string? email = null)
    {
        var userEmail = email ??
            (await userManagementService.GetUserByIdAsync(accessCode.UserId)).Email;

        return accessCode.Type switch
        {
            AccessCodeType.EmailVerification => await notificationWrapper
                                .SendEmailVerificationAsync(userEmail, accessCode.Hash, accessCode.Code),
            _ => await notificationWrapper
                                .SendVerificationCodeAsync(userEmail, accessCode.Code),
        };
    }

}
