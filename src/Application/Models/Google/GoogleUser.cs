using Newtonsoft.Json;

namespace Defender.IdentityService.Application.Models.Google;

[Serializable]
public class GoogleUser
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("email")]
    public string? Email { get; set; }

    [JsonProperty("verified_email")]
    public bool VerifiedEmail { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("given_name")]
    public string? GivenName { get; set; }

    [JsonProperty("family_name")]
    public string? FamilyName { get; set; }

    [JsonProperty("picture")]
    public string? Picture { get; set; }

    [JsonProperty("locale")]
    public string? Locale { get; set; }
}
