using Defender.Common.Configuration.Options;
using Defender.Common.DB.Repositories;
using Defender.IdentityService.Application.Common.Interfaces.Repositories;
using Defender.IdentityService.Domain.Entities;
using Microsoft.Extensions.Options;

namespace Defender.IdentityService.Infrastructure.Repositories;

public class LoginRecordRepository : BaseMongoRepository<LoginRecord>, ILoginRecordRepository
{
    public LoginRecordRepository(IOptions<MongoDbOptions> mongoOption) : base(mongoOption.Value)
    {
    }

    public async Task<LoginRecord> CreateLoginRecordAsync(LoginRecord record)
    {
        return await AddItemAsync(record);
    }

}
