using Defender.Common.Configuration.Options;
using Defender.Common.Repositories;
using Defender.IdentityService.Application.Common.Interfaces.Repositories;
using Defender.IdentityService.Domain.Entities;
using Microsoft.Extensions.Options;

namespace Defender.IdentityService.Infrastructure.Repositories.LoginRecords;

public class LoginRecordRepository : MongoRepository<LoginRecord>, ILoginRecordRepository
{
    public LoginRecordRepository(IOptions<MongoDbOptions> mongoOption) : base(mongoOption.Value)
    {
    }

    #region Default methods

    public async Task<IList<LoginRecord>> GetAllLoginRecordsAsync()
    {
        return await GetItemsAsync();
    }

    public async Task<LoginRecord> GetLoginRecordByIdAsync(Guid id)
    {
        return await GetItemAsync(id);
    }

    public async Task<LoginRecord> CreateLoginRecordAsync(LoginRecord record)
    {
        return await AddItemAsync(record);
    }

    public async Task<LoginRecord> ReplaceLoginRecordAsync(LoginRecord updatedRecord)
    {
        return await ReplaceItemAsync(updatedRecord);
    }

    public async Task RemoveLoginRecordAsync(Guid id)
    {
        await RemoveItemAsync(id);
    }

    #endregion
}
