namespace Defender.IdentityService.Application.Modules;

public class ValidationConstants
{
    public const string PhoneNumberRegex = @"^$|^(1-)?\d{3}-\d{3}-\d{4}$";

    public const int MinPasswordLength = 4;
    public const int MaxPasswordLength = 16;

    public const int MinNicknameLength = 2;
    public const int MaxNicknameLength = 16;

}
