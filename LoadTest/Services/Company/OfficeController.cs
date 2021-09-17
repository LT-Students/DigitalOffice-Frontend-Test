using DigitalOffice.LoadTesting.Models.Company.Requests.Office;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.User
{
    public class OfficeController
    {
        private const string OfficeControllerProdUrl = "https://company.ltdo.xyz/office/";
        private const string OfficeControllerDevUrl = "http://localhost:9816/office/";

        private readonly HttpClient _httpClient;

        private string CreateFindOfficesRequest(int skipCount, int takeCount, bool includeDeactivated = true)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("skipCount", skipCount.ToString());
            url.Add("takeCount", takeCount.ToString());
            url.Add("includeDeactivated", includeDeactivated.ToString());

            return "find?" + url.ToString();
        }

        public OfficeController(string accessToken)
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
            _httpClient.BaseAddress = new Uri(OfficeControllerDevUrl);
#else
            _httpClient.BaseAddress = new Uri(OfficeControllerProdUrl);
#endif
        }

        public async Task<HttpResponseMessage> Find(int skipCount, int takeCount, bool includeDeactivated = true)
        {
            return await _httpClient.GetAsync(CreateFindOfficesRequest(skipCount, takeCount, includeDeactivated));
        }

        public async Task<HttpResponseMessage> Create(CreateOfficeRequest request)
        {
            var httpContent = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PostAsync("create", httpContent);
        }
    }
}
