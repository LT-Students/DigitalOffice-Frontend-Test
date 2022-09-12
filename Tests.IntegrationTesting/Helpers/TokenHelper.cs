using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Tests.IntegrationTesting.Helpers
{
  public class TokenHelper
  {
    private const string AuthServiceLoginUrl = "https://auth.dev.ltdo.xyz/auth/login";

    private class AuthResponse
    {
      public string UserId { get; set; }
      public string AccessToken { get; set; }
      public string RefreshToken { get; set; }
      public string AccessTokenExpiresIn { get; set; }
      public string RefreshTokenExpiresIn { get; set; }
    }

    public static async Task<string> GetToken()
    {
      HttpClient client = new();
      HttpRequestMessage request = new(HttpMethod.Post, AuthServiceLoginUrl);
      request.Content = JsonContent.Create(new
      {
        LoginData = "IntegrationTest",
        Password = "2JH5C7e#$P"
      });

      HttpResponseMessage response = await client.SendAsync(request);
      AuthResponse authResponse = JsonConvert.DeserializeObject<AuthResponse>(await response.Content.ReadAsStringAsync());

      return authResponse.AccessToken;
    }
  }
}
