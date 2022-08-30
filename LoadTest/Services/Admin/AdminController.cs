using LT.DigitalOffice.LoadTesting.Models.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.LoadTesting.Services.Admin
{
  public class AdminController
  {
    private const string AdminControllerProdUrl = "https://admin.ltdo.xyz/admin/";
    private const string AdminControllerDevUrl = "http://localhost:9838/admin/";

    private readonly HttpClient _httpClient;

    private string CreateFindDepartmentsRequest(BaseFindFilter filter)
    {
      NameValueCollection url = System.Web.HttpUtility.ParseQueryString(string.Empty);

      url.Add("skipCount", filter.SkipCount.ToString());
      url.Add("takeCount", filter.TakeCount.ToString());

      return "find?" + url.ToString();
    }

    public AdminController()
    {
      _httpClient = new HttpClient();

#if DEBUG
      _httpClient.BaseAddress = new Uri(AdminControllerDevUrl);
#else
      _httpClient.BaseAddress = new Uri(AdminControllerProdUrl);
#endif
    }

    public Task<HttpResponseMessage> Find(
      int skipCount = 0,
      int takeCount = int.MaxValue)
    {
      return _httpClient.GetAsync(
        CreateFindDepartmentsRequest(
          new BaseFindFilter
          {
            TakeCount = takeCount,
            SkipCount = skipCount
          }));
    }
  }
}
