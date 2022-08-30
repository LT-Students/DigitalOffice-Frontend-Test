using LT.DigitalOffice.LoadTesting.Models.Users.Requests.Gender;
using LT.DigitalOffice.LoadTesting.Models.Users.Requests.Gender.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.User
{
  public class GenderController
  {
    private const string GenderControllerProdUrl = "https://user.ltdo.xyz/gender/";
    private const string GenderControllerDevUrl = "http://localhost:9802/gender/";

    private readonly HttpClient _httpClient;

    private string CreateFindGendersRequest(FindGendersFilter filter)
    {
      NameValueCollection url = System.Web.HttpUtility.ParseQueryString(string.Empty);

      url.Add("nameIncludeSubstring", filter.NameIncludeSubstring?.ToString());
      url.Add("skipCount", filter.SkipCount.ToString());
      url.Add("takeCount", filter.TakeCount.ToString());

      return "get?" + url.ToString();
    }

    public GenderController(string accessToken)
    {
      _httpClient = new HttpClient();

      _httpClient.DefaultRequestHeaders.Add("token", accessToken);

#if DEBUG
      _httpClient.BaseAddress = new Uri(GenderControllerDevUrl);
#else
      _httpClient.BaseAddress = new Uri(GenderControllerProdUrl);
#endif
    }

    public Task<HttpResponseMessage> Find(int skipCount, int takeCount = int.MaxValue)
    {
      return _httpClient.GetAsync(CreateFindGendersRequest(
        new FindGendersFilter
        {
          TakeCount = takeCount,
          SkipCount = skipCount
        }));
    }

    public Task<HttpResponseMessage> Create(CreateGenderRequest request)
    {
      var httpContent = new StringContent(
        JsonConvert.SerializeObject(request),
        Encoding.UTF8,
        "application/json");

      return _httpClient.PostAsync("create", httpContent);
    }
  }
}
