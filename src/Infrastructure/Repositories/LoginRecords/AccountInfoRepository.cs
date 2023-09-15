using Defender.IdentityService.Application.Common.Interfaces.Repositories;
using Defender.IdentityService.Application.Configuration.Options;
using Defender.IdentityService.Domain.Entities;
using Microsoft.Extensions.Options;

namespace Defender.IdentityService.Infrastructure.Repositories.LoginRecords;

public class LoginRecordRepository : MongoRepository<LoginRecord>, ILoginRecordRepository
{
    public LoginRecordRepository(IOptions<MongoDbOption> mongoOption) : base(mongoOption.Value)
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

    public async Task<LoginRecord> UpdateLoginRecordAsync(LoginRecord updatedRecord)
    {
        return await UpdateItemAsync(updatedRecord);
    }

    public async Task RemoveLoginRecordAsync(Guid id)
    {
        await RemoveItemAsync(id);
    }

    #endregion
}
