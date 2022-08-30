using DigitalOffice.LoadTesting.Models.Rights.Requests;
using LT.DigitalOffice.LoadTesting.Models.Rights.Requests.Filters;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.User
{
  public class RolesController
  {
    private const string RolesControllerProdUrl = "https://rights.ltdo.xyz/roles/";
    private const string RolesControllerDevUrl = "http://localhost:9812/roles/";

    private readonly HttpClient _httpClient;

    private string CreateGetRoleRequest(GetRoleFilter filter)
    {
      var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

      url.Add("roleId", filter.RoleId.ToString());
      url.Add("locale", filter.Locale.ToString());

      return "get?" + url.ToString();
    }

    private string CreateFindRolesRequest(FindRolesFilter filter)
    {
      var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

      url.Add("skipCount", filter.SkipCount.ToString());
      url.Add("takeCount", filter.TakeCount.ToString());
      url.Add("includeDeactivated", filter.IncludeDeactivated.ToString());
      url.Add("locale", filter.Locale?.ToString());

      return "find?" + url.ToString();
    }

    public RolesController(string accessToken)
    {
      _httpClient = new HttpClient();

      _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
      _httpClient.BaseAddress = new Uri(RolesControllerDevUrl);
#else
      _httpClient.BaseAddress = new Uri(RolesControllerProdUrl);
#endif
    }

    public Task<HttpResponseMessage> Get(Guid roleId, string locale = "ru")
    {
      return _httpClient.GetAsync(CreateGetRoleRequest(new() { Locale = locale, RoleId = roleId }));
    }

    public Task<HttpResponseMessage> Find(
      int skipCount = 0,
      int takeCount = int.MaxValue,
      bool includeDeactivated = true,
      string locale = "ru")
    {
      return _httpClient.GetAsync(CreateFindRolesRequest(new()
      {
        SkipCount = skipCount,
        TakeCount = takeCount,
        IncludeDeactivated = includeDeactivated,
        Locale = locale
      }));
    }

    public Task<HttpResponseMessage> Create(CreateRoleRequest request)
    {
      var httpContent = new StringContent(
        JsonConvert.SerializeObject(request),
        Encoding.UTF8,
        "application/json");

      return _httpClient.PostAsync("create", httpContent);
    }
  }
}
