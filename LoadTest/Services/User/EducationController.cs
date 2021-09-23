using DigitalOffice.LoadTesting.Helpers;
using DigitalOffice.LoadTesting.Models.Users.Requests.User.Education;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.User
{
    public class EducationController
    {
        private const string EducationControllerProdUrl = "https://user.ltdo.xyz/education/";
        private const string EducationControllerDevUrl = "http://localhost:9802/education/";

        private readonly HttpClient _httpClient;

        private string CreateRemoveEducationRequest(Guid educationId)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("educationId", educationId.ToString());

            return "remove?" + url.ToString();
        }

        private string CreateEditEducationRequest(Guid educationId)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("educationId", educationId.ToString());

            return "edit?" + url.ToString();
        }

        public EducationController(string accessToken)
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
            _httpClient.BaseAddress = new Uri(EducationControllerDevUrl);
#else
            _httpClient.BaseAddress = new Uri(EducationControllerProdUrl);
#endif
        }

        public async Task<HttpResponseMessage> Remove(Guid educationId)
        {
            return await _httpClient.DeleteAsync(CreateRemoveEducationRequest(educationId));
        }

        public async Task<HttpResponseMessage> Create(CreateEducationRequest request)
        {
            var httpContent = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PostAsync("create", httpContent);
        }

        public async Task<HttpResponseMessage> Edit(Guid educationId, List<(string property, string newValue)> changes)
        {
            var httpContent = new StringContent(
                    CreatorJsonPatchDocument.CreateJson(changes),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PatchAsync(CreateEditEducationRequest(educationId), httpContent);
        }
    }
}
