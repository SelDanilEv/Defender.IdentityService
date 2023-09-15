using Defender.Common.Enums;
using Defender.Common.Helpers;
using Defender.Utils;

namespace Defender.IdentityService.Infrastructure.Helpers;

public class PasswordHelper
{
    public static string HashPassword(string password)
        => EncryptionUtils.GetHashSHA256(password, SecretsHelper.GetSecret(Secret.HashSalt));

    public static bool CheckPassword(string password, string hash)
        => hash == EncryptionUtils.GetHashSHA256(password, SecretsHelper.GetSecret(Secret.HashSalt));
}
