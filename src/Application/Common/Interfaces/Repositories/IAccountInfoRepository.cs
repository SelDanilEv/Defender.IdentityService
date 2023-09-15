using Defender.IdentityService.Domain.Entities;

namespace Defender.IdentityService.Application.Common.Interfaces.Repositories;

public interface IAccountInfoRepository
{
    Task<IList<AccountInfo>> GetAllAccountInfosAsync();
    Task<AccountInfo> GetAccountInfoByIdAsync(Guid id);
    Task<AccountInfo> CreateAccountInfoAsync(AccountInfo account);
    Task<AccountInfo> UpdateAccountInfoAsync(AccountInfo updatedAccount);
    Task RemoveAccountInfoAsync(Guid id);
}
