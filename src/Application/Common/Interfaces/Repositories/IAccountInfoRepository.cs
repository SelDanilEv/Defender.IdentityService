using Defender.Common.Models;
using Defender.IdentityService.Domain.Entities;

namespace Defender.IdentityService.Application.Common.Interfaces.Repositories;

public interface IAccountInfoRepository
{
    Task<IList<AccountInfo>> GetAllAccountInfosAsync();
    Task<AccountInfo> GetAccountInfoByIdAsync(Guid id);
    Task<AccountInfo> CreateAccountInfoAsync(AccountInfo account);
    Task UpdateNotificationAsync(Guid id, UpdateModelRequest<AccountInfo> updateModelRequest);
    Task<AccountInfo> ReplaceAccountInfoAsync(AccountInfo account);
    Task RemoveAccountInfoAsync(Guid id);
}
