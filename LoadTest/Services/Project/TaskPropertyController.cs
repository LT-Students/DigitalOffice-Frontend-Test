using DigitalOffice.LoadTesting.Models.Project.Requests;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.Project
{
    class TaskPropertyController
    {
        private const string TaskPropertyControllerProdUrl = "https://project.ltdo.xyz/taskproperty/";
        private const string TaskPropertyControllerDevUrl = "http://localhost:9804/taskproperty/";

        private readonly HttpClient _httpClient;

        private string CreateFindProjectsRequest(int skipCount, int takeCount)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("skipCount", skipCount.ToString());
            url.Add("takeCount", takeCount.ToString());

            return "find?" + url.ToString();
        }

        public TaskPropertyController(string accessToken)
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
            _httpClient.BaseAddress = new Uri(TaskPropertyControllerDevUrl);
#else
            _httpClient.BaseAddress = new Uri(TaskPropertyControllerProdUrl);
#endif
        }

        public async Task<HttpResponseMessage> Find(int skipCount, int takeCount)
        {
            return await _httpClient.GetAsync(CreateFindProjectsRequest(skipCount, takeCount));
        }

        public async Task<HttpResponseMessage> Create(CreateTaskPropertyRequest request)
        {
            var httpContent = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PostAsync("create", httpContent);
        }
    }
}
