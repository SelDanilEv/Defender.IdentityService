using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Application.Common.Interfaces.Repositories;
using Defender.IdentityService.Domain.Entities;

namespace Defender.IdentityService.Infrastructure.Services;

public class LoginHistoryService : ILoginHistoryService
{
    private readonly ILoginRecordRepository _loginRecordRepository;

    public LoginHistoryService(
        ILoginRecordRepository loginRecordRepository)
    {
        _loginRecordRepository = loginRecordRepository;
    }

    public async Task<LoginRecord> AddLoginRecordAsync(LoginRecord loginRecord)
    {
        return await _loginRecordRepository.CreateLoginRecordAsync(loginRecord);
    }
}
