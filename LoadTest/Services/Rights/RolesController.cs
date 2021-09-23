using DigitalOffice.LoadTesting.Models.Rights.Requests;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.User
{
    public class RolesController
    {
        private const string RolesControllerProdUrl = "https://rights.ltdo.xyz/roles/";
        private const string RolesControllerDevUrl = "http://localhost:9812/roles/";

        private readonly HttpClient _httpClient;

        private string CreateGetRoleRequest(Guid roleId)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("roleId", roleId.ToString());

            return "get?" + url.ToString();
        }

        private string CreateFindRolesRequest(int skipCount, int takeCount)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("skipCount", skipCount.ToString());
            url.Add("takeCount", takeCount.ToString());

            return "find?" + url.ToString();
        }

        public RolesController(string accessToken)
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
            _httpClient.BaseAddress = new Uri(RolesControllerDevUrl);
#else
            _httpClient.BaseAddress = new Uri(RolesControllerProdUrl);
#endif
        }

        public async Task<HttpResponseMessage> Get(Guid roleId)
        {
            return await _httpClient.GetAsync(CreateGetRoleRequest(roleId));
        }

        public async Task<HttpResponseMessage> Find(int skipCount, int takeCount)
        {
            return await _httpClient.GetAsync(CreateFindRolesRequest(skipCount, takeCount));
        }

        public async Task<HttpResponseMessage> Create(CreateRoleRequest request)
        {
            var httpContent = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PostAsync("create", httpContent);
        }
    }
}
