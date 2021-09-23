using DigitalOffice.LoadTesting.Helpers;
using DigitalOffice.LoadTesting.Models.Company.Requests.Department;
using DigitalOffice.LoadTesting.Models.Company.Requests.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.User
{
    public class DepartmentController
    {
        private const string DepartmentControllerProdUrl = "https://company.ltdo.xyz/department/";
        private const string DepartmentControllerDevUrl = "http://localhost:9816/department/";

        private readonly HttpClient _httpClient;

        private string CreateGetDepartmentRequest(GetDepartmentFilter filter)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("departmentId", filter.DepartmentId.ToString());
            url.Add("includeDepartment", filter.IncludeProjects.ToString());
            url.Add("includePosition", filter.IncludeUsers.ToString());

            return "get?" + url.ToString();
        }

        private string CreateFindDepartmentsRequest(int skipCount, int takeCount, bool includeDeactivated = true)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("skipCount", skipCount.ToString());
            url.Add("takeCount", takeCount.ToString());
            url.Add("includeDeactivated", includeDeactivated.ToString());

            return "find?" + url.ToString();
        }

        private string CreateEditDepartmentRequest(Guid departmentId)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("departmentId", departmentId.ToString());

            return "edit?" + url.ToString();
        }

        public DepartmentController(string accessToken)
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
            _httpClient.BaseAddress = new Uri(DepartmentControllerDevUrl);
#else
            _httpClient.BaseAddress = new Uri(DepartmentControllerProdUrl);
#endif
        }

        public async Task<HttpResponseMessage> Get(Guid departmentId)
        {
            return await _httpClient.GetAsync(CreateGetDepartmentRequest(new GetDepartmentFilter { DepartmentId = departmentId }));
        }

        public async Task<HttpResponseMessage> Find(int skipCount, int takeCount, bool includeDeactivated = true)
        {
            return await _httpClient.GetAsync(CreateFindDepartmentsRequest(skipCount, takeCount, includeDeactivated));
        }

        public async Task<HttpResponseMessage> Create(CreateDepartmentRequest request)
        {
            var httpContent = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PostAsync("create", httpContent);
        }

        public async Task<HttpResponseMessage> Edit(Guid departmentId, List<(string property, string newValue)> changes)
        {
            var httpContent = new StringContent(
                    CreatorJsonPatchDocument.CreateJson(changes),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PatchAsync(CreateEditDepartmentRequest(departmentId), httpContent);
        }
    }
}
