using DigitalOffice.LoadTesting.Models.Time.Filters;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.Time
{
  public class ImportController
  {
    private const string ImportControllerProdUrl = "https://time.ltdo.xyz/import/";
    private const string ImportControllerDevUrl = "http://localhost:9806/import/";

    private readonly HttpClient _httpClient;

    private string CreateGetRequest(ImportStatFilter filter)
    {
      var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

      url.Add("DepartmentId", filter.DepartmentId.ToString());
      url.Add("ProjectId", filter.ProjectId.ToString());
      url.Add("Year", filter.Year.ToString());
      url.Add("Month", filter.Month.ToString());

      return "get?" + url.ToString();
    }

    public ImportController(string accessToken)
    {
      _httpClient = new HttpClient();

      _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
      _httpClient.BaseAddress = new Uri(ImportControllerDevUrl);
#else
      _httpClient.BaseAddress = new Uri(ImportControllerProdUrl);
#endif
    }

    public Task<HttpResponseMessage> Get(ImportStatFilter filter)
    {
      return _httpClient.GetAsync(CreateGetRequest(filter));
    }
  }
}
