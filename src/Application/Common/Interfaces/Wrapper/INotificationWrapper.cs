namespace Defender.IdentityService.Application.Common.Interfaces.Wrapper;
public interface INotificationWrapper
{
    Task<string> SendEmailVerificationAsync(string email, string verificationLink);
}
