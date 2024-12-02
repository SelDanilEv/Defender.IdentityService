using Defender.IdentityService.Application.Models.Google;

namespace Defender.IdentityService.Application.Common.Interfaces.Services;

public interface IGoogleTokenParsingService
{
    Task<GoogleUser> GetGoogleUserAsync(string token);
}
