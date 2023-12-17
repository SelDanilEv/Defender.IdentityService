using Defender.IdentityService.Domain.Entities;

namespace Defender.IdentityService.Application.Common.Interfaces;

public interface IAccountManagementService
{
    Task<AccountInfo> GetAccountByIdAsync(Guid accountId);
    Task<AccountInfo> GetOrCreateAccountAsync(Guid accountId, string password = "");
    Task<AccountInfo> GetAccountWithPasswordAsync(Guid accountId, string password);
    Task<AccountInfo> UpdateEmailVerificationAsync(Guid accountId, bool isEmailVerified);
    Task<AccountInfo> ChangePasswordAsync(Guid accountId, string newPassword);
    Task<AccountInfo> BlockAsync(Guid accountId, bool doBlockUser);
}
