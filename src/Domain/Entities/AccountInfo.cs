using Defender.Common.Entities.AccountInfo;

namespace Defender.IdentityService.Domain.Entities;

public record AccountInfo : BaseAccountInfo
{
    public string? PasswordHash { get; set; }
    public bool IsPhoneVerified { get; set; }
    public bool IsEmailVerified { get; set; }
    public bool IsBlocked { get; set; }
}
