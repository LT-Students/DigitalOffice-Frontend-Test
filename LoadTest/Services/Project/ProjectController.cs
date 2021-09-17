using DigitalOffice.LoadTesting.Helpers;
using DigitalOffice.LoadTesting.Models.Project.Requests;
using DigitalOffice.LoadTesting.Models.Project.Requests.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.Project
{
    public class ProjectController
    {
        private const string ProjectControllerProdUrl = "https://project.ltdo.xyz/project/";
        private const string ProjectControllerDevUrl = "http://localhost:9804/project/";

        private readonly HttpClient _httpClient;

        private string CreateGetProjectRequest(GetProjectFilter filter)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("projectId", filter.ProjectId.ToString());
            url.Add("IncludeFiles", filter.IncludeFiles.ToString());
            url.Add("IncludeImages", filter.IncludeImages.ToString());
            url.Add("IncludeUsers", filter.IncludeUsers.ToString());

            return "get?" + url.ToString();
        }

        private string CreateFindProjectsRequest(int skipCount, int takeCount, FindProjectsFilter filter)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("skipCount", skipCount.ToString());
            url.Add("takeCount", takeCount.ToString());
            url.Add("DepartmentId", filter.DepartmentId.ToString());

            return "find?" + url.ToString();
        }

        private string CreateEditProjectRequest(Guid projectId)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("projectId", projectId.ToString());

            return "edit?" + url.ToString();
        }

        public ProjectController(string accessToken)
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
            _httpClient.BaseAddress = new Uri(ProjectControllerDevUrl);
#else
            _httpClient.BaseAddress = new Uri(ProjectControllerProdUrl);
#endif
        }

        public async Task<HttpResponseMessage> Get(GetProjectFilter filter)
        {
            return await _httpClient.GetAsync(CreateGetProjectRequest(filter));
        }

        public async Task<HttpResponseMessage> Find(int skipCount, int takeCount, FindProjectsFilter filter)
        {
            return await _httpClient.GetAsync(CreateFindProjectsRequest(skipCount, takeCount, filter));
        }

        public async Task<HttpResponseMessage> Create(CreateProjectRequest request)
        {
            var httpContent = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PostAsync("create", httpContent);
        }

        public async Task<HttpResponseMessage> Edit(Guid projectId, List<(string property, string newValue)> changes)
        {
            var httpContent = new StringContent(
                    CreatorJsonPatchDocument.CreateJson(changes),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PatchAsync(CreateEditProjectRequest(projectId), httpContent);
        }
    }
}
