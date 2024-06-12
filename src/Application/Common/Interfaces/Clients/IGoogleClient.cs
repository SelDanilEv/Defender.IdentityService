using Defender.IdentityService.Application.Models.Google;

namespace Defender.IdentityService.Application.Common.Interfaces.Clients;

public interface IGoogleClient
{
    Task<GoogleUser> GetTokenInfo(string token);
}
