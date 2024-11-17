using Defender.IdentityService.Application.Helpers.LocalSecretHelper;
using Defender.Utils;

namespace Defender.IdentityService.Application.Helpers;

public class PasswordHelper
{
    public static async Task<string> HashPassword(string password)
        => EncryptionUtils.GetHashSHA256(password, await LocalSecretsHelper.GetSecretAsync(LocalSecret.HashSalt));

    public static async Task<bool> CheckPassword(string password, string hash)
        => hash == EncryptionUtils.GetHashSHA256(password, await LocalSecretsHelper.GetSecretAsync(LocalSecret.HashSalt));
}
