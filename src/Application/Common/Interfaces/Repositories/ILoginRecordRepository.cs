using Defender.IdentityService.Domain.Entities;

namespace Defender.IdentityService.Application.Common.Interfaces.Repositories;

public interface ILoginRecordRepository
{
    Task<LoginRecord> CreateLoginRecordAsync(LoginRecord record);
}
