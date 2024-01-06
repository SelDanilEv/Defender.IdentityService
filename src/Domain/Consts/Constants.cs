namespace Defender.IdentityService.Domain.Consts;
internal class Constants
{
    public const int AccessCodeMinValue = 100000;
    public const int AccessCodeMaxValue = 999999;

    public const int AccessCodeDefaultValidTimeMinutes = 10;
    public const int AccessCodeDefaultAttemts = 3;
}
