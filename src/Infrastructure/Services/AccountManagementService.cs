using Defender.Common.DB.Model;
using Defender.Common.Errors;
using Defender.Common.Exceptions;
using Defender.Common.Interfaces;
using Defender.Common.Models;
using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Application.Common.Interfaces.Repositories;
using Defender.IdentityService.Application.Modules.Account.Commands;
using Defender.IdentityService.Domain.Entities;
using Defender.IdentityService.Infrastructure.Helpers;
using MongoDB.Driver.Core.Operations;

namespace Defender.IdentityService.Infrastructure.Services;

public class AccountManagementService : IAccountManagementService
{
    private readonly IAccountInfoRepository _accountInfoRepository;
    private readonly IAccountAccessor _accountAccessor;
    private readonly IAccessCodeService _accessCodeService;

    public AccountManagementService(
        IAccessCodeService accessCodeService,
        IAccountInfoRepository accountInfoRepository,
        IAccountAccessor accountAccessor)
    {
        _accessCodeService = accessCodeService;
        _accountInfoRepository = accountInfoRepository;
        _accountAccessor = accountAccessor;
    }

    public async Task<AccountInfo> GetAccountByIdAsync(Guid accountId)
    {
        return await _accountInfoRepository.GetAccountInfoByIdAsync(accountId);
    }

    public async Task<AccountInfo> GetOrCreateAccountAsync(Guid accountId, string password = "")
    {
        var accountInfo = await this.GetAccountByIdAsync(accountId);

        if (accountInfo == null)
        {
            accountInfo = await CreateDefaultUserAccount(accountId, password);

            accountInfo = await _accountInfoRepository.CreateAccountInfoAsync(accountInfo);
        }

        return accountInfo;
    }

    public async Task<AccountInfo> GetAccountWithPasswordAsync(Guid accountId, string password)
    {
        var accountInfo = await this.GetAccountByIdAsync(accountId);

        if (!await PasswordHelper.CheckPassword(password, accountInfo.PasswordHash))
        {
            throw new ServiceException(
                ErrorCodeHelper.GetErrorCode(ErrorCode.BR_ACC_InvalidPassword));
        }

        return accountInfo;
    }


    public async Task<AccountInfo> ChangePasswordAsync(Guid accountId, string newPassword)
    {
        var userAccount = await GetOrCreateAccountAsync(accountId);

        var currentUser = _accountAccessor.AccountInfo;

        if (currentUser.IsSuperAdmin)
        {
            return await ChangePasswordAsync(accountId, newPassword);
        }
        else if (currentUser.IsAdmin)
        {
            if (userAccount.Id != currentUser.Id && userAccount.IsAdmin)
            {
                throw new ForbiddenAccessException(ErrorCode.BR_ACC_AdminCannotChangeAdminPassword);
            }

            return await PrivateChangePasswordAsync(accountId, newPassword);
        }
        else
        {
            if (userAccount.Id != currentUser.Id)
            {
                throw new ForbiddenAccessException(ErrorCode.BR_ACC_UserCanUpdateOnlyOwnAccount);
            }

            return await PrivateChangePasswordAsync(accountId, newPassword);
        }
    }

    public async Task VerifyEmailAsync(int hash, int code)
    {
        var (isVerified, userId) = await _accessCodeService.VerifyAccessCode(hash, code);

        if (!isVerified)
        {
            throw new ServiceException(ErrorCode.BR_ACC_CodeWasNotVerified);
        }

        await UpdateEmailVerificationAsync(userId, true);
    }

    public async Task<AccountInfo> UpdateAccountInfoAsync(UpdateAccountCommand updateAccountCommand)
    {
        var updateRequest = updateAccountCommand.CreateUpdateRequest();

        return await _accountInfoRepository.UpdateAccountInfoAsync(updateRequest);
    }

    public async Task<AccountInfo> UpdateEmailVerificationAsync(
        Guid accountId, bool isEmailVerified)
    {
        var updateRequest = UpdateModelRequest<AccountInfo>
            .Init(accountId);

        updateRequest.UpdateField(x => x.IsEmailVerified, isEmailVerified);

        return await _accountInfoRepository.UpdateAccountInfoAsync(updateRequest);
    }

    public async Task<AccountInfo> BlockAsync(Guid accountId, bool doBlockUser)
    {
        var updateRequest = UpdateModelRequest<AccountInfo>
            .Init(accountId);

        updateRequest
            .UpdateField(x => x.IsBlocked, doBlockUser);

        return await _accountInfoRepository.UpdateAccountInfoAsync(updateRequest);
    }

    private async Task<AccountInfo> PrivateChangePasswordAsync(Guid accountId, string newPassword)
    {
        var updateRequest = UpdateModelRequest<AccountInfo>
            .Init(accountId);

        updateRequest
            .UpdateField(x => x.PasswordHash, await PasswordHelper.HashPassword(newPassword));

        return await _accountInfoRepository.UpdateAccountInfoAsync(updateRequest);
    }

    private async Task<AccountInfo> CreateDefaultUserAccount(Guid accountId, string password)
    {
        var passwordHash = string.IsNullOrWhiteSpace(password) ?
                                await PasswordHelper.HashPassword(accountId.ToString()) :
                                await PasswordHelper.HashPassword(password);

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
