using LT.DigitalOffice.LoadTesting.Models.Company.Requests.ContractSubject;
using LT.DigitalOffice.LoadTesting.Models.Company.Requests.ContractSubject.Filters;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.LoadTesting.Services.Company
{
  public class ContractSubjectController
  {
    private const string ContractSubjectControllerProdUrl = "https://company.ltdo.xyz/contractsubject/";
    private const string ContractSubjectControllerDevUrl = "http://localhost:9816/contractsubject/";

    private readonly HttpClient _httpClient;

    private string CreateFindContractSubjectsRequest(FindContractSubjectFilter filter)
    {
      var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

      url.Add("isActive", filter.IsActive?.ToString());
      url.Add("skipCount", filter.SkipCount.ToString());
      url.Add("takeCount", filter.TakeCount.ToString());

      return "find?" + url.ToString();
    }

    public ContractSubjectController(string accessToken)
    {
      _httpClient = new HttpClient();

      _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
      _httpClient.BaseAddress = new Uri(ContractSubjectControllerDevUrl);
#else
      _httpClient.BaseAddress = new Uri(ContractSubjectControllerProdUrl);
#endif
    }

    public Task<HttpResponseMessage> Find(FindContractSubjectFilter filter)
    {
      return _httpClient.GetAsync(CreateFindContractSubjectsRequest(filter));
    }

    public Task<HttpResponseMessage> Create(CreateContractSubjectRequest request)
    {
      var httpContent = new StringContent(
        JsonConvert.SerializeObject(request),
        Encoding.UTF8,
        "application/json");

      return _httpClient.PostAsync("create", httpContent);
    }
  }
}
