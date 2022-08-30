using DigitalOffice.LoadTesting.Helpers;
using LT.DigitalOffice.LoadTesting.Models.Project.Requests.Project;
using LT.DigitalOffice.LoadTesting.Models.Project.Requests.Project.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace DigitalOffice.LoadTesting.Services.Project
{
  public class ProjectController
  {
    private const string ProjectControllerProdUrl = "https://project.ltdo.xyz/project/";
    private const string ProjectControllerDevUrl = "http://localhost:9804/project/";

    private readonly HttpClient _httpClient;

    private string CreateGetProjectRequest(GetProjectFilter filter)
    {
      var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

      url.Add("projectId", filter.ProjectId.ToString());
      url.Add("includeDepartment", filter.IncludeDepartment.ToString());
      url.Add("includeProjectUsers", filter.IncludeProjectUsers.ToString());

      return "get?" + url.ToString();
    }

    private string CreateFindProjectsRequest(FindProjectsFilter filter)
    {
      var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

      url.Add("skipCount", filter.SkipCount.ToString());
      url.Add("takeCount", filter.TakeCount.ToString());
      url.Add("isAscendingSort", filter.IsAscendingSort?.ToString());
      url.Add("projectStatus", filter.ProjectStatus?.ToString());
      url.Add("nameIncludeSubstring", filter.NameIncludeSubstring?.ToString());
      url.Add("includeDepartment", filter.IncludeDepartment.ToString());
      url.Add("userId", filter.UserId?.ToString());
      url.Add("departmentId", filter.DepartmentId?.ToString());

      return "find?" + url.ToString();
    }

    private string CreateEditProjectRequest(Guid projectId)
    {
      var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

      url.Add("projectId", projectId.ToString());

      return "edit?" + url.ToString();
    }

    public ProjectController(string accessToken)
    {
      _httpClient = new HttpClient();

      _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
      _httpClient.BaseAddress = new Uri(ProjectControllerDevUrl);
#else
      _httpClient.BaseAddress = new Uri(ProjectControllerProdUrl);
#endif
    }

    public Task<HttpResponseMessage> Get(GetProjectFilter filter)
    {
      return _httpClient.GetAsync(CreateGetProjectRequest(filter));
    }

    public Task<HttpResponseMessage> Find(FindProjectsFilter filter)
    {
      return _httpClient.GetAsync(CreateFindProjectsRequest(filter));
    }

    public Task<HttpResponseMessage> Create(CreateProjectRequest request)
    {
      var httpContent = new StringContent(
        JsonConvert.SerializeObject(request),
        Encoding.UTF8,
        "application/json");

      return _httpClient.PostAsync("create", httpContent);
    }

    public Task<HttpResponseMessage> Edit(Guid projectId, List<(string property, string newValue)> changes)
    {
      var httpContent = new StringContent(
        CreatorJsonPatchDocument.CreateJson(changes),
        Encoding.UTF8,
        "application/json");

      return _httpClient.PatchAsync(CreateEditProjectRequest(projectId), httpContent);
    }
  }
}
