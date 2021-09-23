using DigitalOffice.LoadTesting.Models.Project.Requests;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.Project
{
    public class UserController
    {
        private const string UserControllerProdUrl = "https://project.ltdo.xyz/user/";
        private const string UserControllerDevUrl = "http://localhost:9804/user/";

        private readonly HttpClient _httpClient;

        private string CreateRemoveUserRequest(Guid projectId, Guid[] usersIds)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("projectId", projectId.ToString());
            
            foreach(Guid userId in usersIds)
            {
                url.Add("userIds[]", userId.ToString());
            }

            return "removeUsersFromProject?" + url.ToString();
        }

        public UserController(string accessToken)
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
            _httpClient.BaseAddress = new Uri(UserControllerDevUrl);
#else
            _httpClient.BaseAddress = new Uri(UserControllerProdUrl);
#endif
        }

        public async Task<HttpResponseMessage> RemoveUsersFromProject(Guid projectId, Guid[] userIds)
        {
            return await _httpClient.GetAsync(CreateRemoveUserRequest(projectId, userIds));
        }

        public async Task<HttpResponseMessage> AddUsersToProject(AddUsersToProjectRequest request)
        {
            var httpContent = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PostAsync("create", httpContent);
        }
    }
}
