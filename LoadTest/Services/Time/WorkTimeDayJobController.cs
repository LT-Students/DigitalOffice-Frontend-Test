using DigitalOffice.LoadTesting.Helpers;
using DigitalOffice.LoadTesting.Models.Time.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.Time
{
    public class WorkTimeDayJobController
    {
        private const string WorkTimeDayJobControllerProdUrl = "https://time.ltdo.xyz/worktimedayjob/";
        private const string WorkTimeDayJobControllerDevUrl = "http://localhost:9806/worktimedayjob/";

        private readonly HttpClient _httpClient;

        private string CreateEditWorkTimeDayJobRequest(Guid jobId)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("worktimedayjobid", jobId.ToString());

            return "edit?" + url.ToString();
        }

        public WorkTimeDayJobController(string accessToken)
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
            _httpClient.BaseAddress = new Uri(WorkTimeDayJobControllerDevUrl);
#else
            _httpClient.BaseAddress = new Uri(WorkTimeDayJobControllerProdUrl);
#endif
        }

        public async Task<HttpResponseMessage> Create(CreateWorkTimeDayJobRequest request)
        {
            var httpContent = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PostAsync("create", httpContent);
        }

        public async Task<HttpResponseMessage> Edit(Guid workTimeDayJobId, List<(string property, string newValue)> changes)
        {
            var httpContent = new StringContent(
                    CreatorJsonPatchDocument.CreateJson(changes),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PatchAsync(CreateEditWorkTimeDayJobRequest(workTimeDayJobId), httpContent);
        }
    }
}
