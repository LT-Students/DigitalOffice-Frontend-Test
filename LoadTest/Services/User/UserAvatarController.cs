using LT.DigitalOffice.LoadTesting.Models.Users.Requests.Avatar;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.LoadTesting.Services.User
{
  public class UserAvatarController
  {
    private const string AvatarControllerProdUrl = "https://user.ltdo.xyz/avatar/";
    private const string AvatarControllerDevUrl = "http://localhost:9802/avatar/";

    private readonly HttpClient _httpClient;

    private string CreateGetAvatarRequest(Guid userId)
    {
      NameValueCollection url = System.Web.HttpUtility.ParseQueryString(string.Empty);

      url.Add("userId", userId.ToString());

      return "get?" + url.ToString();
    }

    public UserAvatarController(string accessToken)
    {
      _httpClient = new HttpClient();

      _httpClient.DefaultRequestHeaders.Add("token", accessToken);

#if DEBUG
      _httpClient.BaseAddress = new Uri(AvatarControllerDevUrl);
#else
      _httpClient.BaseAddress = new Uri(AvatarControllerProdUrl);
#endif
    }

    public Task<HttpResponseMessage> Get(Guid userId)
    {
      return _httpClient.GetAsync(CreateGetAvatarRequest(userId));
    }

    public Task<HttpResponseMessage> Create(CreateAvatarRequest request)
    {
      var httpContent = new StringContent(
        JsonConvert.SerializeObject(request),
        Encoding.UTF8,
        "application/json");

      return _httpClient.PostAsync("create", httpContent);
    }
  }
}
