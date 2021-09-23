using DigitalOffice.LoadTesting.Helpers;
using DigitalOffice.LoadTesting.Models.Company.Requests.Company.Filters;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.User
{
    public class CompanyController
    {
        private const string CompanyControllerProdUrl = "https://company.ltdo.xyz/company/";
        private const string CompanyControllerDevUrl = "http://localhost:9816/company/";

        private readonly HttpClient _httpClient;

        private string CreateGetCompanyRequest(GetCompanyFilter filter)
        {
            var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

            url.Add("includeDepartment", filter.IncludeDepartments.ToString());
            url.Add("includePosition", filter.IncludeOffices.ToString());
            url.Add("includeOffice", filter.IncludePositions.ToString());
            url.Add("includeRole", filter.IncludeSmtpCredentials.ToString());

            return "get?" + url.ToString();
        }

        public CompanyController(string accessToken)
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
            _httpClient.BaseAddress = new Uri(CompanyControllerDevUrl);
#else
            _httpClient.BaseAddress = new Uri(CompanyControllerProdUrl);
#endif
        }

        public async Task<HttpResponseMessage> Get()
        {
            return await _httpClient.GetAsync(CreateGetCompanyRequest(new GetCompanyFilter()));
        }

        public async Task<HttpResponseMessage> Edit(List<(string property, string newValue)> changes)
        {
            var httpContent = new StringContent(
                    CreatorJsonPatchDocument.CreateJson(changes),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PatchAsync("edit", httpContent);
        }
    }
}
