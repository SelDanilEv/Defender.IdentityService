using Defender.Common.Enums;
using Defender.Common.Helpers;
using Defender.Utils;

namespace Defender.IdentityService.Infrastructure.Helpers;

public class PasswordHelper
{
    public static async Task<string> HashPassword(string password)
        => EncryptionUtils.GetHashSHA256(password, await SecretsHelper.GetSecretAsync(Secret.HashSalt));

    public static async Task<bool> CheckPassword(string password, string hash)
        => hash == EncryptionUtils.GetHashSHA256(password, await SecretsHelper.GetSecretAsync(Secret.HashSalt));
}
