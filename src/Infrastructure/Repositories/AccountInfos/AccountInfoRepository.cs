using Defender.IdentityService.Application.Common.Interfaces.Repositories;
using Defender.IdentityService.Application.Configuration.Options;
using Defender.IdentityService.Domain.Entities;
using Microsoft.Extensions.Options;

namespace Defender.IdentityService.Infrastructure.Repositories.AccountInfos;

public class AccountInfoRepository : MongoRepository<AccountInfo>, IAccountInfoRepository
{
    public AccountInfoRepository(IOptions<MongoDbOption> mongoOption) : base(mongoOption.Value)
    {
    }

    #region Default methods

    public async Task<IList<AccountInfo>> GetAllAccountInfosAsync()
    {
        return await GetItemsAsync();
    }

    public async Task<AccountInfo> GetAccountInfoByIdAsync(Guid id)
    {
        return await GetItemAsync(id);
    }

    public async Task<AccountInfo> CreateAccountInfoAsync(AccountInfo user)
    {
        return await AddItemAsync(user);
    }

    public async Task<AccountInfo> UpdateAccountInfoAsync(AccountInfo updatedAccountInfo)
    {
        return await UpdateItemAsync(updatedAccountInfo);
    }

    public async Task RemoveAccountInfoAsync(Guid id)
    {
        await RemoveItemAsync(id);
    }

    #endregion
}
