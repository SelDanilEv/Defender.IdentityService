using Defender.Common.Enums;

namespace Defender.IdentityService.Application.Models.ApiRequests;

public record UpdateAccountInfoRequest(
        Guid Id,
        Role? Role,
        bool? IsPhoneVerified,
        bool? IsEmailVerified,
        bool? IsBlocked);
