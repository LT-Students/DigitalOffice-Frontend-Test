using DigitalOffice.LoadTesting.Helpers;
using LT.DigitalOffice.LoadTesting.Models.Department.Requests.Department;
using LT.DigitalOffice.LoadTesting.Models.Department.Requests.Department.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.LoadTesting.Services.Department
{
  public class DepartmentController
  {
    private const string DepartmentControllerProdUrl = "https://department.ltdo.xyz/department/";
    private const string DepartmentControllerDevUrl = "http://localhost:9830/department/";

    private readonly HttpClient _httpClient;

    private string CreateGetDepartmentRequest(GetDepartmentFilter filter)
    {
      NameValueCollection url = System.Web.HttpUtility.ParseQueryString(string.Empty);

      url.Add("departmentId", filter.DepartmentId.ToString());
      url.Add("includeUsers", filter.IncludeUsers.ToString());
      url.Add("includeCategory", filter.IncludeUsers.ToString());

      return "get?" + url.ToString();
    }

    private string CreateFindDepartmentsRequest(FindDepartmentFilter filter)
    {
      NameValueCollection url = System.Web.HttpUtility.ParseQueryString(string.Empty);

      url.Add("isAscendingSort", filter.IsAscendingSort.ToString());
      url.Add("nameIncludeSubstring", filter.NameIncludeSubstring?.ToString());
      url.Add("isActive", filter.IsActive.ToString());
      url.Add("skipCount", filter.SkipCount.ToString());
      url.Add("takeCount", filter.TakeCount.ToString());

      return "find?" + url.ToString();
    }

    private string CreateEditUsersRequest(Guid departmentId)
    {
      var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

      url.Add("departmentId", departmentId.ToString());

      return "edit?" + url.ToString();
    }

    public DepartmentController(string accessToken)
    {
      _httpClient = new HttpClient();

      _httpClient.DefaultRequestHeaders.Add("token", accessToken);

#if DEBUG
      _httpClient.BaseAddress = new Uri(DepartmentControllerDevUrl);
#else
      _httpClient.BaseAddress = new Uri(DepartmentControllerProdUrl);
#endif
    }

    public Task<HttpResponseMessage> Get(Guid departmentId)
    {
      return _httpClient.GetAsync(CreateGetDepartmentRequest(new GetDepartmentFilter { DepartmentId = departmentId }));
    }

    public Task<HttpResponseMessage> Find(
      int skipCount = 0,
      int takeCount = int.MaxValue,
      bool? isAscendingSort = true,
      bool? isActive = null)
    {
      return _httpClient.GetAsync(
        CreateFindDepartmentsRequest(
          new FindDepartmentFilter
          {
            TakeCount = takeCount,
            SkipCount = skipCount,
            IsAscendingSort = isAscendingSort,
            IsActive = isActive
          }));
    }

    public Task<HttpResponseMessage> Create(CreateDepartmentRequest request)
    {
      var httpContent = new StringContent(
        JsonConvert.SerializeObject(request),
        Encoding.UTF8,
        "application/json");

      return _httpClient.PostAsync("create", httpContent);
    }

    public Task<HttpResponseMessage> Edit(Guid userId, List<(string property, string newValue)> changes)
    {
      StringContent httpContent = new StringContent(
        CreatorJsonPatchDocument.CreateJson(changes),
        Encoding.UTF8,
        "application/json");

      return _httpClient.PatchAsync(CreateEditUsersRequest(userId), httpContent);
    }
  }
}
