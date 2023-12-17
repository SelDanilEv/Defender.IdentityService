using Defender.IdentityService.Domain.Entities;

namespace Defender.IdentityService.Application.Common.Interfaces;

public interface IAccountVerificationService
{
    Task<string> SendVerificationEmailAsync(Guid userId);
    Task VerifyEmailAsync(int hash, int code);
}
