using DigitalOffice.LoadTesting.Helpers;
using DigitalOffice.LoadTesting.Models.Message.Requests.Workspace;
using DigitalOffice.LoadTesting.Models.Message.Requests.Workspace.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.Message
{
    public class WorkspaceController
    {
        private const string WorkspaceControllerProdUrl = "https://message.ltdo.xyz/workspace/";
        private const string WorkspaceControllerDevUrl = "http://localhost:9810/workspace/";

        private readonly HttpClient _httpClient;

        private string CreateGetWorkspaceRequest(GetWorkspaceFilter filter)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("workspaceId", filter.WorkspaceId.ToString());
            url.Add("IncludeChannels", filter.IncludeChannels.ToString());
            url.Add("IncludeUsers", filter.IncludeUsers.ToString());

            return "get?" + url.ToString();
        }

        private string CreateFindWorkspacesRequest(FindWorkspaceFilter filter)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("skipCount", filter.SkipCount.ToString());
            url.Add("takeCount", filter.TakeCount.ToString());
            url.Add("includeDeactivated", filter.IncludeDeactivated.ToString());

            return "find?" + url.ToString();
        }

        private string CreateEditWorkspaceRequest(Guid workspaceId)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("workspaceId", workspaceId.ToString());

            return "edit?" + url.ToString();
        }

        public WorkspaceController(string accessToken)
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
            _httpClient.BaseAddress = new Uri(WorkspaceControllerDevUrl);
#else
            _httpClient.BaseAddress = new Uri(WorkspaceControllerProdUrl);
#endif
        }

        public async Task<HttpResponseMessage> Get(GetWorkspaceFilter filter)
        {
            return await _httpClient.GetAsync(CreateGetWorkspaceRequest(filter));
        }

        public async Task<HttpResponseMessage> Find(FindWorkspaceFilter filter)
        {
            return await _httpClient.GetAsync(CreateFindWorkspacesRequest(filter));
        }

        public async Task<HttpResponseMessage> Create(CreateWorkspaceRequest request)
        {
            var httpContent = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PostAsync("create", httpContent);
        }

        public async Task<HttpResponseMessage> Edit(Guid workspaceId, List<(string property, string newValue)> changes)
        {
            var httpContent = new StringContent(
                    CreatorJsonPatchDocument.CreateJson(changes),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PatchAsync(CreateEditWorkspaceRequest(workspaceId), httpContent);
        }
    }
}
