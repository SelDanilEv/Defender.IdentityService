using Defender.Common.Models;
using Defender.IdentityService.Domain.Entities;

namespace Defender.IdentityService.Application.Common.Interfaces.Repositories;

public interface IAccountInfoRepository
{
    Task<AccountInfo> GetAccountInfoByIdAsync(Guid id);
    Task<AccountInfo> CreateAccountInfoAsync(AccountInfo account);
    Task<AccountInfo> UpdateAccountInfoAsync(UpdateModelRequest<AccountInfo> updateModelRequest);
}
