using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Application.Models.Google;
using Defender.IdentityService.Infrastructure.Clients.Interfaces;

namespace Defender.IdentityService.Infrastructure.Services;
public class GoogleTokenParsingService : IGoogleTokenParsingService
{
    private readonly IGoogleClient _googleClient;

    public GoogleTokenParsingService(IGoogleClient googleClient)
    {
        _googleClient = googleClient;
    }

    public async Task<GoogleUser> GetGoogleUserAsync(string token)
    {
        if (token == null)
            throw new ArgumentNullException(nameof(token));

        return await _googleClient.GetTokenInfo(token);
    }
}
