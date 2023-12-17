using Defender.Common.Errors;
using Defender.Common.Exceptions;
using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Application.Models.Google;
using Defender.IdentityService.Infrastructure.Clients.Google;

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
            throw new ServiceException(ErrorCode.VL_InvalidRequest);

        return await _googleClient.GetTokenInfo(token);
    }
}
