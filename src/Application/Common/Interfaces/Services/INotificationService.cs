using Defender.IdentityService.Domain.Entities;

namespace Defender.IdentityService.Application.Common.Interfaces.Services;

public interface INotificationService
{
    public Task<string> SendVerificationCodeAsync(AccessCode accessCode, string? email = null);
}
