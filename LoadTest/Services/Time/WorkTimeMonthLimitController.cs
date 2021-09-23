using DigitalOffice.LoadTesting.Helpers;
using DigitalOffice.LoadTesting.Models.Time.Filters;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.Time
{
    public class WorkTimeMonthLimitController
    {
        private const string WorkTimeMonthLimitControllerProdUrl = "https://time.ltdo.xyz/worktimemonthlimit/";
        private const string WorkTimeMonthLimitControllerDevUrl = "http://localhost:9806/worktimemonthlimit/";

        private readonly HttpClient _httpClient;

        private string CreateFindWorkTimeMonthLimitRequest(FindWorkTimeMonthLimitsFilter filter, int skipCount, int takeCount)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("Year", filter.Year.ToString());
            url.Add("Month", filter.Month.ToString());
            url.Add("skipCount", skipCount.ToString());
            url.Add("takeCount", takeCount.ToString());

            return "find?" + url.ToString();
        }

        private string CreateEditWorkTimeMonthLimitRequest(Guid workTimeMonthLimitId)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("workTimeMonthLimitId", workTimeMonthLimitId.ToString());

            return "edit?" + url.ToString();
        }

        public WorkTimeMonthLimitController(string accessToken)
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
            _httpClient.BaseAddress = new Uri(WorkTimeMonthLimitControllerDevUrl);
#else
            _httpClient.BaseAddress = new Uri(WorkTimeMonthLimitControllerProdUrl);
#endif
        }

        public async Task<HttpResponseMessage> Find(FindWorkTimeMonthLimitsFilter filter, int skipCount, int takeCount)
        {
            return await _httpClient.GetAsync(CreateFindWorkTimeMonthLimitRequest(filter, skipCount, takeCount));
        }

        public async Task<HttpResponseMessage> Edit(Guid workTimeMonthLimitId, List<(string property, string newValue)> changes)
        {
            var httpContent = new StringContent(
                    CreatorJsonPatchDocument.CreateJson(changes),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PatchAsync(CreateEditWorkTimeMonthLimitRequest(workTimeMonthLimitId), httpContent);
        }
    }
}
