using DigitalOffice.LoadTesting.Helpers;
using DigitalOffice.LoadTesting.Models.Users.Requests.User.Certificates;
using DigitalOffice.LoadTesting.Models.Users.Requests.User.Education;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.User
{
    public class CertificateController
    {
        private const string CertificateControllerProdUrl = "https://user.ltdo.xyz/certificate/";
        private const string CertificateControllerDevUrl = "http://localhost:9802/certificate/";

        private readonly HttpClient _httpClient;

        private string CreateRemoveCertificateRequest(Guid certificateId)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("certificateId", certificateId.ToString());

            return "remove?" + url.ToString();
        }

        private string CreateEditCertificateRequest(Guid certificateId)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("certificateId", certificateId.ToString());

            return "edit?" + url.ToString();
        }

        public CertificateController(string accessToken)
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
            _httpClient.BaseAddress = new Uri(CertificateControllerDevUrl);
#else
            _httpClient.BaseAddress = new Uri(CertificateControllerProdUrl);
#endif
        }

        public async Task<HttpResponseMessage> Remove(Guid certificateId)
        {
            return await _httpClient.DeleteAsync(CreateRemoveCertificateRequest(certificateId));
        }

        public async Task<HttpResponseMessage> Create(CreateCertificateRequest request)
        {
            var httpContent = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PostAsync("create", httpContent);
        }

        public async Task<HttpResponseMessage> Edit(Guid certificateId, List<(string property, string newValue)> changes)
        {
            var httpContent = new StringContent(
                    CreatorJsonPatchDocument.CreateJson(changes),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PatchAsync(CreateEditCertificateRequest(certificateId), httpContent);
        }
    }
}
