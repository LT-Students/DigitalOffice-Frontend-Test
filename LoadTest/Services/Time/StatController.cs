using DigitalOffice.LoadTesting.Models.Time.Filters;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.Time
{
  public class StatController
  {
    private const string StatControllerProdUrl = "https://time.ltdo.xyz/stat/";
    private const string StatControllerDevUrl = "http://localhost:9806/stat/";

    private readonly HttpClient _httpClient;

    private string CreateFindStatRequest(FindStatFilter filter)
    {
      var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

      url.Add("skipCount", filter.SkipCount.ToString());
      url.Add("takeCount", filter.TakeCount.ToString());
      url.Add("ProjectId", filter.ProjectId?.ToString());
      url.Add("Year", filter.Year.ToString());
      url.Add("Month", filter.Month.ToString());
      url.Add("ascendingSort", filter.AscendingSort?.ToString());
      url.Add("nameIncludeSubstring", filter.NameIncludeSubstring?.ToString());

      if (filter.DepartmentsIds is not null && filter.DepartmentsIds.Any())
      {
        foreach (Guid departmentId in filter.DepartmentsIds)
        {
          url.Add("departmentsIds", departmentId.ToString());
        }
      }

      return "find?" + url.ToString();
    }

    public StatController(string accessToken)
    {
      _httpClient = new HttpClient();

      _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
      _httpClient.BaseAddress = new Uri(StatControllerDevUrl);
#else
      _httpClient.BaseAddress = new Uri(StatControllerProdUrl);
#endif
    }

    public Task<HttpResponseMessage> Find(FindStatFilter filter)
    {
      return _httpClient.GetAsync(CreateFindStatRequest(filter));
    }
  }
}
