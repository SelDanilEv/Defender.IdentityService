using Defender.Common.Models;
using Defender.IdentityService.Domain.Entities;

namespace Defender.IdentityService.Application.Common.Interfaces.Repositories;

public interface IAccessCodeRepository
{
    Task<AccessCode> GetAccessCodeByHashAsync(int hash);
    Task<AccessCode> CreateAccessCodeAsync(AccessCode record);
    Task<AccessCode> UpdateAccessCodeAsync(UpdateModelRequest<AccessCode> request);
}
