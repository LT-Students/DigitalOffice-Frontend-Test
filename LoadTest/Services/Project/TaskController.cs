using DigitalOffice.LoadTesting.Helpers;
using DigitalOffice.LoadTesting.Models.Project.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.Project
{
    public class TaskController
    {
        private const string TaskControllerProdUrl = "https://project.ltdo.xyz/task/";
        private const string TaskControllerDevUrl = "http://localhost:9804/task/";

        private readonly HttpClient _httpClient;

        private string CreateGetTaskRequest(Guid taskId)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("taskId", taskId.ToString());

            return "get?" + url.ToString();
        }

        private string CreateFindTasksRequest(int skipCount, int takeCount)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("skipCount", skipCount.ToString());
            url.Add("takeCount", takeCount.ToString());

            return "find?" + url.ToString();
        }

        private string CreateEditTaskRequest(Guid taskId)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("taskId", taskId.ToString());

            return "edit?" + url.ToString();
        }

        public TaskController(string accessToken)
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
            _httpClient.BaseAddress = new Uri(TaskControllerDevUrl);
#else
            _httpClient.BaseAddress = new Uri(TaskControllerProdUrl);
#endif
        }

        public async Task<HttpResponseMessage> Get(Guid taskId)
        {
            return await _httpClient.GetAsync(CreateGetTaskRequest(taskId));
        }

        public async Task<HttpResponseMessage> Find(int skipCount, int takeCount)
        {
            return await _httpClient.GetAsync(CreateFindTasksRequest(skipCount, takeCount));
        }

        public async Task<HttpResponseMessage> Create(CreateTaskRequest request)
        {
            var httpContent = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PostAsync("create", httpContent);
        }

        public async Task<HttpResponseMessage> Edit(Guid taskId, List<(string property, string newValue)> changes)
        {
            var httpContent = new StringContent(
                    CreatorJsonPatchDocument.CreateJson(changes),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PatchAsync(CreateEditTaskRequest(taskId), httpContent);
        }
    }
}
