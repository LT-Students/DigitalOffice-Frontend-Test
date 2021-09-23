using DigitalOffice.LoadTesting.Helpers;
using DigitalOffice.LoadTesting.Models.Time.Filters;
using DigitalOffice.LoadTesting.Models.Time.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.Time
{
    public class LeaveTimeController
    {
        private const string LeaveTimeControllerProdUrl = "https://time.ltdo.xyz/leavetime/";
        private const string LeaveTimeControllerDevUrl = "http://localhost:9806/leavetime/";

        private readonly HttpClient _httpClient;

        private string CreateFindLeaveTimeRequest(FindLeaveTimesFilter filter)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("skipCount", filter.SkipCount.ToString());
            url.Add("takeCount", filter.TakeCount.ToString());
            url.Add("UserId", filter.UserId.ToString());
            url.Add("StartTime", filter.StartTime.ToString());
            url.Add("EndTime", filter.EndTime.ToString());
            url.Add("IncludeDeactivated", filter.IncludeDeactivated.ToString());

            return "find?" + url.ToString();
        }

        private string CreateEditLeaveTimeRequest(Guid leaveTimeId)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("leaveTimeId", leaveTimeId.ToString());

            return "edit?" + url.ToString();
        }

        public LeaveTimeController(string accessToken)
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
            _httpClient.BaseAddress = new Uri(LeaveTimeControllerDevUrl);
#else
            _httpClient.BaseAddress = new Uri(LeaveTimeControllerProdUrl);
#endif
        }

        public async Task<HttpResponseMessage> Add(CreateLeaveTimeRequest request)
        {
            var httpContent = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PostAsync("create", httpContent);
        }

        public async Task<HttpResponseMessage> Find(FindLeaveTimesFilter filter)
        {
            return await _httpClient.GetAsync(CreateFindLeaveTimeRequest(filter));
        }

        public async Task<HttpResponseMessage> Edit(Guid leaveTimeId, List<(string property, string newValue)> changes)
        {
            var httpContent = new StringContent(
                    CreatorJsonPatchDocument.CreateJson(changes),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PatchAsync(CreateEditLeaveTimeRequest(leaveTimeId), httpContent);
        }
    }
}
