using Defender.IdentityService.Domain.Entities;

namespace Defender.IdentityService.Application.Common.Interfaces;

public interface INotificationService
{
    public Task<string> SendVerificationCodeAsync(AccessCode accessCode);
}
