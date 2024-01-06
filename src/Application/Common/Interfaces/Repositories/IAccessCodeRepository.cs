using Defender.Common.DB.Model;
using Defender.IdentityService.Domain.Entities;

namespace Defender.IdentityService.Application.Common.Interfaces.Repositories;

public interface IAccessCodeRepository
{
    Task<AccessCode> GetAccessCodeByHashAsync(int hash);
    Task<AccessCode> GetAccessCodeByUserIdAsync(Guid userId);
    Task<AccessCode> CreateAccessCodeAsync(AccessCode record);
    Task<AccessCode> UpdateAccessCodeAsync(UpdateModelRequest<AccessCode> request);
}
