namespace Defender.IdentityService.Application.Common.Interfaces.Wrapper;
public interface INotificationWrapper
{
    Task<string> SendEmailVerificationAsync(string email, int hash, int code);
    Task<string> SendVerificationCodeAsync(string email, int code);
}
