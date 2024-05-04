using Defender.Common.Configuration.Options;
using Defender.Common.DB.Model;
using Defender.Common.DB.Repositories;
using Defender.IdentityService.Application.Common.Interfaces.Repositories;
using Defender.IdentityService.Domain.Entities;
using Microsoft.Extensions.Options;

namespace Defender.IdentityService.Infrastructure.Repositories;

public class AccountInfoRepository : BaseMongoRepository<AccountInfo>, IAccountInfoRepository
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
        return await UpdateItemAsync(updateModelRequest);
    }
}
