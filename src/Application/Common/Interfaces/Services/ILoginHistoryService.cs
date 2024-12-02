using Defender.IdentityService.Domain.Entities;

namespace Defender.IdentityService.Application.Common.Interfaces.Services;

public interface ILoginHistoryService
{
    Task<LoginRecord> AddLoginRecordAsync(LoginRecord loginRecord);
}
