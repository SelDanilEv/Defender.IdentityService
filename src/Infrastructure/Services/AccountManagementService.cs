using Defender.Common.Errors;
using Defender.Common.Exceptions;
using Defender.Common.Models;
using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Application.Common.Interfaces.Repositories;
using Defender.IdentityService.Domain.Entities;
using Defender.IdentityService.Infrastructure.Helpers;

namespace Defender.IdentityService.Infrastructure.Services;

public class AccountManagementService : IAccountManagementService
{
    private readonly IAccountInfoRepository _accountInfoRepository;


    public AccountManagementService(
        IAccountInfoRepository accountInfoRepository)
    {
        _accountInfoRepository = accountInfoRepository;
    }

    public async Task<AccountInfo> GetOrCreateAccountAsync(Guid accountId, string password = "")
    {
        var accountInfo = await this.GetAccountInfoByIdAsync(accountId);

        if (accountInfo == null)
        {
            accountInfo = CreateDefaultUserAccount(accountId, password);

            accountInfo = await _accountInfoRepository.CreateAccountInfoAsync(accountInfo);
        }

        return accountInfo;
    }
    public async Task<AccountInfo> GetAccountWithPasswordAsync(Guid accountId, string password)
    {
        var accountInfo = await this.GetAccountInfoByIdAsync(accountId);

        if (!PasswordHelper.CheckPassword(password, accountInfo.PasswordHash))
        {
            throw new ServiceException(ErrorCodeHelper.GetErrorCode(ErrorCode.BR_ACC_InvalidPassword));
        }

        return accountInfo;
    }

    public async Task ChangePasswordAsync(Guid accountId, string newPassword)
    {
        var user = await GetOrCreateAccountAsync(accountId, newPassword);

        user.PasswordHash = PasswordHelper.HashPassword(newPassword);

        await _accountInfoRepository.UpdateAccountInfoAsync(user);
    }

    public async Task BlockAsync(Guid accountId, bool doBlockUser)
    {
        var user = await GetOrCreateAccountAsync(accountId);

        user.IsBlocked = doBlockUser;

        await _accountInfoRepository.UpdateAccountInfoAsync(user);
    }

    private async Task<AccountInfo> GetAccountInfoByIdAsync(Guid accountId)
    {
        var accountInfo = await _accountInfoRepository.GetAccountInfoByIdAsync(accountId);

        if (accountInfo.IsBlocked)
        {
            throw new ServiceException(ErrorCode.BR_ACC_UserIsBlocked);
        }

        return accountInfo;
    }


    private AccountInfo CreateDefaultUserAccount(Guid accountId, string password)
    {
        var passwordHash = string.IsNullOrWhiteSpace(password) ?
                                PasswordHelper.HashPassword(accountId.ToString()) :
                                PasswordHelper.HashPassword(password);

        return new AccountInfo()
        {
            Id = accountId,
            PasswordHash = passwordHash,
            IsPhoneVerified = false,
            IsEmailVerified = false,
            IsBlocked = false,
            Roles = new List<string>() { Roles.User, Roles.Guest },
        };
    }
}
