using Defender.IdentityService.Domain.Entities;

namespace Defender.IdentityService.Application.Common.Interfaces;

public interface ILoginHistoryService
{
    Task<LoginRecord> AddLoginRecordAsync(LoginRecord loginRecord);
}
