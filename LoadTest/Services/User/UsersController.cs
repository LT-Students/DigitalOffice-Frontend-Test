using DigitalOffice.LoadTesting.Helpers;
using DigitalOffice.LoadTesting.Models.Users.Requests.User;
using DigitalOffice.LoadTesting.Models.Users.Requests.User.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.User
{
    public class UsersController
    {
        private const string UsersControllerProdUrl = "https://user.ltdo.xyz/users/";
        private const string UsersControllerDevUrl = "http://localhost:9802/users/";

        private readonly HttpClient _httpClient;

        private string CreateGetUserRequest(GetUserFilter filter)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("UserId", filter.UserId.ToString());
            url.Add("includeDepartment", filter.IncludeDepartment.ToString());
            url.Add("includePosition", filter.IncludePosition.ToString());
            url.Add("includeOffice", filter.IncludeOffice.ToString());
            url.Add("includeRole", filter.IncludeRole.ToString());
            url.Add("includeProjects", filter.IncludeProjects.ToString());
            url.Add("includeEducations", filter.IncludeEducations.ToString());
            url.Add("includeCommunications", filter.IncludeCommunications.ToString());
            url.Add("includeCertificates", filter.IncludeCertificates.ToString());
            url.Add("includeAchievements", filter.IncludeAchievements.ToString());
            url.Add("includeImages", filter.IncludeImages.ToString());

            return "get?" + url.ToString();
        }

        private string CreateFindUsersRequest(int skipCount, int takeCount)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("skipCount", skipCount.ToString());
            url.Add("takeCount", takeCount.ToString());

            return "find?" + url.ToString();
        }

        private string CreateEditUsersRequest(Guid userId)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("userId", userId.ToString());

            return "edit?" + url.ToString();
        }

        public UsersController(string accessToken)
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
            _httpClient.BaseAddress = new Uri(UsersControllerDevUrl);
#else
            _httpClient.BaseAddress = new Uri(UsersControllerProdUrl);
#endif
        }

        public async Task<HttpResponseMessage> Get(Guid userId)
        {
            return await _httpClient.GetAsync(CreateGetUserRequest(new GetUserFilter { UserId = userId }));
        }

        public async Task<HttpResponseMessage> Find(int skipCount, int takeCount)
        {
            return await _httpClient.GetAsync(CreateFindUsersRequest(skipCount, takeCount));
        }

        public async Task<HttpResponseMessage> Create(CreateUserRequest request)
        {
            var httpContent = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PostAsync("create", httpContent);
        }

        public async Task<HttpResponseMessage> Edit(Guid userId, List<(string property, string newValue)> changes)
        {
            var httpContent = new StringContent(
                    CreatorJsonPatchDocument.CreateJson(changes),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PatchAsync(CreateEditUsersRequest(userId), httpContent);
        }
    }
}
