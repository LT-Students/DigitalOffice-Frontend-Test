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
            url.Add("UserId", filter.UserId.ToString());
            url.Add("ProjectId", filter.ProjectId.ToString());
            url.Add("Year", filter.Year.ToString());
            url.Add("Month", filter.Month.ToString());
            url.Add("IncludeDayJobs", filter.IncludeDayJobs.ToString());
            url.Add("IncludeDeactivated", filter.IncludeDeactivated.ToString());

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

        public async Task<HttpResponseMessage> Find(FindWorkTimesFilter filter)
        {
            return await _httpClient.GetAsync(CreateFindWorkTimeRequest(filter));
        }

        public async Task<HttpResponseMessage> Edit(Guid workTimeId, List<(string property, string newValue)> changes)
        {
            var httpContent = new StringContent(
                    CreatorJsonPatchDocument.CreateJson(changes),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PatchAsync(CreateEditWorkTimeRequest(workTimeId), httpContent);
        }
    }
}
