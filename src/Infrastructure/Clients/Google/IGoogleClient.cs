using Defender.IdentityService.Application.Models.Google;

namespace Defender.IdentityService.Infrastructure.Clients.Google;
public interface IGoogleClient
{
    Task<GoogleUser> GetTokenInfo(string token);
}
