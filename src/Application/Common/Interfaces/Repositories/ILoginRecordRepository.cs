using Defender.IdentityService.Domain.Entities;

namespace Defender.IdentityService.Application.Common.Interfaces.Repositories;

public interface ILoginRecordRepository
{
    Task<IList<LoginRecord>> GetAllLoginRecordsAsync();
    Task<LoginRecord> GetLoginRecordByIdAsync(Guid id);
    Task<LoginRecord> CreateLoginRecordAsync(LoginRecord record);
    Task<LoginRecord> ReplaceLoginRecordAsync(LoginRecord loginRecord);
    Task RemoveLoginRecordAsync(Guid id);
}
