using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.User
{
  public class RightsController
  {
    private const string RightsControllerProdUrl = "https://rights.ltdo.xyz/rights/";
    private const string RightsControllerDevUrl = "http://localhost:9812/rights/";

    private readonly HttpClient _httpClient;

    private string CreateGetRightsRequest(string locale = "ru")
    {
      var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

      url.Add("locale", locale);

      return "get?" + url.ToString();
    }

    public RightsController(string accessToken)
    {
      _httpClient = new HttpClient();

      _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
      _httpClient.BaseAddress = new Uri(RightsControllerDevUrl);
#else
      _httpClient.BaseAddress = new Uri(RightsControllerProdUrl);
#endif
    }

    public Task<HttpResponseMessage> GetRightsList()
    {
      return _httpClient.GetAsync(CreateGetRightsRequest());
    }
  }
}
