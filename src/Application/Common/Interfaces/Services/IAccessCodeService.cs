using Defender.IdentityService.Domain.Entities;
using Defender.IdentityService.Domain.Enum;

namespace Defender.IdentityService.Application.Common.Interfaces;

public interface IAccessCodeService
{
    Task<AccessCode> CreateAccessCodeAsync(Guid accountId, AccessCodeType accessCodeType);
    Task<bool> VerifyAccessCode(Guid accountId, int code, AccessCodeType accessCodeType);
    Task<(bool, Guid)> VerifyAccessCode(int hash, int code, AccessCodeType accessCodeType);
}
