using DigitalOffice.LoadTesting.Helpers;
using DigitalOffice.LoadTesting.Models.Company.Requests.Position;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.User
{
    public class PositionController
    {
        private const string PositionControllerProdUrl = "https://company.ltdo.xyz/position/";
        private const string PositionControllerDevUrl = "http://localhost:9816/position/";

        private readonly HttpClient _httpClient;

        private string CreateGetPositionRequest(Guid positionId)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("positionId", positionId.ToString());

            return "get?" + url.ToString();
        }

        private string CreateFindPositionsRequest(int skipCount, int takeCount, bool includeDeactivated = true)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("skipCount", skipCount.ToString());
            url.Add("takeCount", takeCount.ToString());
            url.Add("includeDeactivated", includeDeactivated.ToString());

            return "find?" + url.ToString();
        }

        private string CreateEditPositionRequest(Guid positionId)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("positionId", positionId.ToString());

            return "edit?" + url.ToString();
        }

        public PositionController(string accessToken)
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
            _httpClient.BaseAddress = new Uri(PositionControllerDevUrl);
#else
            _httpClient.BaseAddress = new Uri(PositionControllerProdUrl);
#endif
        }

        public async Task<HttpResponseMessage> Get(Guid positionId)
        {
            return await _httpClient.GetAsync(CreateGetPositionRequest(positionId));
        }

        public async Task<HttpResponseMessage> Find(int skipCount, int takeCount, bool includeDeactivated = true)
        {
            return await _httpClient.GetAsync(CreateFindPositionsRequest(skipCount, takeCount, includeDeactivated));
        }

        public async Task<HttpResponseMessage> Create(CreatePositionRequest request)
        {
            var httpContent = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PostAsync("create", httpContent);
        }

        public async Task<HttpResponseMessage> Edit(Guid positionId, List<(string property, string newValue)> changes)
        {
            var httpContent = new StringContent(
                    CreatorJsonPatchDocument.CreateJson(changes),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PatchAsync(CreateEditPositionRequest(positionId), httpContent);
        }
    }
}
