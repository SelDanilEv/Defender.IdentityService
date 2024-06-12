using Defender.Common.Errors;
using Defender.Common.Exceptions;
using Defender.IdentityService.Application.Common.Interfaces;
using Defender.IdentityService.Application.Models.Google;
using Defender.IdentityService.Application.Common.Interfaces.Clients;

namespace Defender.IdentityService.Application.Services;
public class GoogleTokenParsingService(IGoogleClient googleClient) : IGoogleTokenParsingService
{
    public async Task<GoogleUser> GetGoogleUserAsync(string token)
    {
        if (token == null)
            throw new ServiceException(ErrorCode.VL_InvalidRequest);

        return await googleClient.GetTokenInfo(token);
    }
}
