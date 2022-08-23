using DigitalOffice.LoadTesting.Helpers;
using DigitalOffice.LoadTesting.Models.Time.Filters;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.Time
{
  public class WorkTimeController
  {
    private const string WorkTimeControllerProdUrl = "https://time.ltdo.xyz/worktime/";
    private const string WorkTimeControllerDevUrl = "http://localhost:9806/worktime/";

    private readonly HttpClient _httpClient;

    private string CreateFindWorkTimeRequest(FindWorkTimesFilter filter)
    {
      var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

      url.Add("skipCount", filter.SkipCount.ToString());
      url.Add("takeCount", filter.TakeCount.ToString());
      url.Add("userId", filter.UserId?.ToString());
      url.Add("projectId", filter.ProjectId?.ToString());
      url.Add("year", filter.Year?.ToString());
      url.Add("month", filter.Month?.ToString());
      url.Add("includeDayJobs", filter.IncludeDayJobs?.ToString());
      url.Add("includeDeactivated", filter.IncludeDeactivated?.ToString());

      return "find?" + url.ToString();
    }

    private string CreateEditWorkTimeRequest(Guid workTimeId)
    {
      var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

      url.Add("workTimeId", workTimeId.ToString());

      return "edit?" + url.ToString();
    }

    public WorkTimeController(string accessToken)
    {
      _httpClient = new HttpClient();

      _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
      _httpClient.BaseAddress = new Uri(WorkTimeControllerDevUrl);
#else
      _httpClient.BaseAddress = new Uri(WorkTimeControllerProdUrl);
#endif
    }

    public Task<HttpResponseMessage> Find(FindWorkTimesFilter filter)
    {
      return _httpClient.GetAsync(CreateFindWorkTimeRequest(filter));
    }

    public Task<HttpResponseMessage> Edit(Guid workTimeId, List<(string property, string newValue)> changes)
    {
      var httpContent = new StringContent(
        CreatorJsonPatchDocument.CreateJson(changes),
        Encoding.UTF8,
        "application/json");

      return _httpClient.PatchAsync(CreateEditWorkTimeRequest(workTimeId), httpContent);
    }
  }
}
