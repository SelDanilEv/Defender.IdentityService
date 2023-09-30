using Defender.IdentityService.Domain.Entities;

namespace Defender.IdentityService.Application.Common.Interfaces;

public interface IAccessCodeService
{
    Task<AccessCode> CreateEmailVerificationAccessCodeAsync(Guid accountId);
    Task<(bool,Guid)> VerifyAccessCode(int hash, int code);
}
