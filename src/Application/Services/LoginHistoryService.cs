using Defender.IdentityService.Application.Common.Interfaces.Repositories;
using Defender.IdentityService.Application.Common.Interfaces.Services;
using Defender.IdentityService.Domain.Entities;

namespace Defender.IdentityService.Application.Services;

public class LoginHistoryService(
    ILoginRecordRepository loginRecordRepository)
    : ILoginHistoryService
{
    public async Task<LoginRecord> AddLoginRecordAsync(LoginRecord loginRecord)
    {
        return await loginRecordRepository.CreateLoginRecordAsync(loginRecord);
    }
}
