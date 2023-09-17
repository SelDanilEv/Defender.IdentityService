using Defender.IdentityService.Application.Common.Exceptions;
using Defender.IdentityService.Application.Models.Google;
using Defender.IdentityService.Infrastructure.Clients.Google;
using Newtonsoft.Json;

namespace Defender.IdentityService.Infrastructure.Clients;

public partial class GoogleClient : IGoogleClient
{
    private readonly HttpClient _httpClient;

    public GoogleClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GoogleUser> GetTokenInfo(string token)
    {
        if (token == null)
            throw new ArgumentNullException(nameof(token));

        var url = $"oauth2/v1/userinfo?alt=json&access_token={token}";

        try
        {
            using (var request = new HttpRequestMessage())
            {
                request.Method = new HttpMethod("GET");
                request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

                var response = await _httpClient.SendAsync(request);

                if ((int)response.StatusCode == 200)
                {
                    var responseText = await response.Content.ReadAsStringAsync();

                    try
                    {
                        return JsonConvert.DeserializeObject<GoogleUser>(responseText);
                    }
                    catch (JsonSerializationException exception)
                    {
                        var message = "Could not deserialize the response body string as " + typeof(GoogleUser).FullName + ".";
                        throw new InvalidCastException(message);
                    }
                }

                throw new GoogleClientException();
            }
        }
        finally
        {
            _httpClient.Dispose();
        }
    }

}
