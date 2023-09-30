using Defender.Common.Configuration.Options;
using Defender.Common.Models;
using Defender.Common.Repositories;
using Defender.IdentityService.Application.Common.Interfaces.Repositories;
using Defender.IdentityService.Domain.Entities;
using Microsoft.Extensions.Options;

namespace Defender.IdentityService.Infrastructure.Repositories.AccountInfos;

public class AccountInfoRepository : MongoRepository<AccountInfo>, IAccountInfoRepository
{
    public AccountInfoRepository(IOptions<MongoDbOptions> mongoOption) : base(mongoOption.Value)
    {
    }

    public async Task<AccountInfo> GetAccountInfoByIdAsync(Guid id)
    {
        return await GetItemAsync(id);
    }

    public async Task<AccountInfo> CreateAccountInfoAsync(AccountInfo user)
    {
        return await AddItemAsync(user);
    }

    public async Task<AccountInfo> UpdateAccountInfoAsync(UpdateModelRequest<AccountInfo> updateModelRequest)
    {
        return await UpdateItemAsync(updateModelRequest.ModelId, updateModelRequest.BuildUpdateDefinition());
    }
}
