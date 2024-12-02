using Defender.Common.Consts;
using Defender.Common.DB.Model;
using Defender.Common.Enums;
using Defender.Common.Errors;
using Defender.Common.Exceptions;
using Defender.Common.Helpers;
using Defender.Common.Interfaces;
using Defender.IdentityService.Application.Common.Interfaces.Repositories;
using Defender.IdentityService.Application.Common.Interfaces.Services;
using Defender.IdentityService.Application.Helpers;
using Defender.IdentityService.Application.Models.ApiRequests;
using Defender.IdentityService.Domain.Entities;
using Defender.IdentityService.Domain.Enum;

namespace Defender.IdentityService.Application.Services;

public class AccountManagementService(
        IAccessCodeService accessCodeService,
        IAccountInfoRepository accountInfoRepository,
        ICurrentAccountAccessor currentAccountAccessor)
    : IAccountManagementService
{
    public async Task<AccountInfo> GetAccountByIdAsync(Guid accountId)
    {
        return await accountInfoRepository.GetAccountInfoByIdAsync(accountId);
    }

    public async Task<AccountInfo> GetOrCreateAccountAsync(Guid accountId, string password = "")
    {
        var accountInfo = await this.GetAccountByIdAsync(accountId);

        if (accountInfo == null)
        {
            accountInfo = await CreateDefaultUserAccount(accountId, password);

            accountInfo = await accountInfoRepository.CreateAccountInfoAsync(accountInfo);
        }

        return accountInfo;
    }

    public async Task<AccountInfo> GetAccountWithPasswordAsync(Guid accountId, string password)
    {
        var accountInfo = await this.GetAccountByIdAsync(accountId);

        if (accountInfo == null)
        {
            throw new ServiceException(
                ErrorCodeHelper.GetErrorCode(ErrorCode.BR_ACC));
        }

        if (!await PasswordHelper.CheckPassword(password, accountInfo.PasswordHash))
        {
            throw new ServiceException(
                ErrorCodeHelper.GetErrorCode(ErrorCode.BR_ACC_InvalidPassword));
        }

        return accountInfo;
    }


    public async Task<AccountInfo> ChangePasswordAsync(Guid accountId, string newPassword)
    {
        return await PrivateChangePasswordAsync(accountId, newPassword);
    }

    public async Task VerifyEmailAsync(int hash, int code)
    {
        var (isVerified, userId) = await accessCodeService.VerifyAccessCode(hash, code, AccessCodeType.EmailVerification);

        if (!isVerified)
        {
            throw new ServiceException(ErrorCode.BR_ACC_CodeWasNotVerified);
        }

        await UpdateEmailVerificationAsync(userId, true);
    }

    public async Task<AccountInfo> UpdateAccountInfoAsync(UpdateAccountInfoRequest updateRequest)
    {
        var updateModelRequest = CreateUpdateModelRequest(updateRequest);

        return await accountInfoRepository.UpdateAccountInfoAsync(updateModelRequest);
    }

    public async Task<AccountInfo> UpdateEmailVerificationAsync(
        Guid accountId, bool isEmailVerified)
    {
        var updateRequest = UpdateModelRequest<AccountInfo>
            .Init(accountId);

        updateRequest.Set(x => x.IsEmailVerified, isEmailVerified);

        return await accountInfoRepository.UpdateAccountInfoAsync(updateRequest);
    }

    public async Task<AccountInfo> BlockAsync(Guid accountId, bool doBlockUser)
    {
        var updateRequest = UpdateModelRequest<AccountInfo>
            .Init(accountId);

        updateRequest
            .Set(x => x.IsBlocked, doBlockUser);

        return await accountInfoRepository.UpdateAccountInfoAsync(updateRequest);
    }

    private async Task<AccountInfo> PrivateChangePasswordAsync(Guid accountId, string newPassword)
    {
        var updateRequest = UpdateModelRequest<AccountInfo>
            .Init(accountId);

        updateRequest
            .Set(x => x.PasswordHash, await PasswordHelper.HashPassword(newPassword));

        return await accountInfoRepository.UpdateAccountInfoAsync(updateRequest);
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


    private UpdateModelRequest<AccountInfo> CreateUpdateModelRequest(
        UpdateAccountInfoRequest request)
    {
        var isSuperAdmin = currentAccountAccessor.GetHighestRole() == Role.SuperAdmin;

        var updateRequest = UpdateModelRequest<AccountInfo>
            .Init(request.Id)
            .SetIfNotNull(x => x.IsPhoneVerified, request.IsPhoneVerified)
            .SetIfNotNull(x => x.IsEmailVerified, request.IsEmailVerified)
            .SetIfNotNull(x => x.Roles, isSuperAdmin && request.Role.HasValue
                ? RolesHelper.GetRolesList(request.Role.Value)
                : null)
            .SetIfNotNull(x => x.IsBlocked, request.IsBlocked);

        return updateRequest;
    }
}
