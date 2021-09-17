using DigitalOffice.LoadTesting.Helpers;
using DigitalOffice.LoadTesting.Models.Users.Requests.User.Communication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.User
{
    public class CommunicationController
    {
        private const string CommunicationControllerProdUrl = "https://user.ltdo.xyz/communication/";
        private const string CommunicationControllerDevUrl = "http://localhost:9802/communication/";

        private readonly HttpClient _httpClient;

        private string CreateRemoveCommunicationRequest(Guid communicationId)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("communicationId", communicationId.ToString());

            return "remove?" + url.ToString();
        }

        private string CreateEditCommunicationRequest(Guid communicationId)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("communicationId", communicationId.ToString());

            return "edit?" + url.ToString();
        }

        public CommunicationController(string accessToken)
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
            _httpClient.BaseAddress = new Uri(CommunicationControllerDevUrl);
#else
            _httpClient.BaseAddress = new Uri(CommunicationControllerProdUrl);
#endif
        }

        public async Task<HttpResponseMessage> Remove(Guid communicationId)
        {
            return await _httpClient.DeleteAsync(CreateRemoveCommunicationRequest(communicationId));
        }

        public async Task<HttpResponseMessage> Create(CreateCommunicationRequest request)
        {
            var httpContent = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PostAsync("create", httpContent);
        }

        public async Task<HttpResponseMessage> Edit(Guid communicationId, List<(string property, string newValue)> changes)
        {
            var httpContent = new StringContent(
                    CreatorJsonPatchDocument.CreateJson(changes),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PatchAsync(CreateEditCommunicationRequest(communicationId), httpContent);
        }
    }
}
