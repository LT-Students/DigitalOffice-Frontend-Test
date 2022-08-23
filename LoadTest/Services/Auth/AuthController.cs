using DigitalOffice.LoadTesting.Models.Auth.Requests;
using DigitalOffice.LoadTesting.Models.Auth.Responses;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.Auth
{
  public class AuthController
  {
    private const string AuthControllerProdUrl = "https://auth.ltdo.xyz/auth/";
    private const string AuthControllerDevUrl = "http://localhost:9818/auth/";

    private readonly HttpClient _httpClient;

    public AuthController()
    {
      _httpClient = new();

#if DEBUG
      _httpClient.BaseAddress = new Uri(AuthControllerDevUrl);
#else
      _httpClient.BaseAddress = new Uri(AuthControllerProdUrl);
#endif
    }

    public async Task<LoginResult> Auth(string loginData, string password)
    {
      var loginRequest = new LoginRequest
      {
        LoginData = loginData,
        Password = password
      };

      var httpContent = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");

      HttpResponseMessage response = await _httpClient.PostAsync("login", httpContent);

      return JsonConvert
        .DeserializeObject<LoginResult>(
          Encoding.Default.GetString(await response.Content.ReadAsByteArrayAsync()));
    }

    public async Task<LoginResult> RefreshToken(string refreshToken)
    {
      var refreshRequest = new RefreshRequest
      {
        RefreshToken = refreshToken
      };

      var httpContent = new StringContent(JsonConvert.SerializeObject(refreshRequest), Encoding.UTF8, "application/json");
      var response = await _httpClient.PostAsync("refresh", httpContent);

      return JsonConvert
        .DeserializeObject<LoginResult>(
          Encoding.Default.GetString(await response.Content.ReadAsByteArrayAsync()));
    }
  }
}
