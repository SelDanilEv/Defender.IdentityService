using Defender.IdentityService.Application.Models.ApiRequests;
using Defender.IdentityService.Domain.Entities;

namespace Defender.IdentityService.Application.Common.Interfaces;

public interface IAccountManagementService
{
    Task<AccountInfo> GetAccountByIdAsync(Guid accountId);
    Task<AccountInfo> GetOrCreateAccountAsync(Guid accountId, string password = "");
    Task<AccountInfo> GetAccountWithPasswordAsync(Guid accountId, string password);
    Task<AccountInfo> UpdateEmailVerificationAsync(Guid accountId, bool isEmailVerified);
    Task<AccountInfo> UpdateAccountInfoAsync(UpdateAccountInfoRequest updateRequest);
    Task<AccountInfo> ChangePasswordAsync(Guid accountId, string newPassword);
    Task VerifyEmailAsync(int hash, int code);
    Task<AccountInfo> BlockAsync(Guid accountId, bool doBlockUser);
}
