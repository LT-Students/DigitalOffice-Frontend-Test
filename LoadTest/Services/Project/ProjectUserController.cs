using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Services.WebApi;
using LT.DigitalOffice.LoadTesting.Models.Project.Requests.User;

namespace DigitalOffice.LoadTesting.Services.Project
{
  public class ProjectUserController
  {
    private const string UserControllerProdUrl = "https://project.ltdo.xyz/user/";
    private const string UserControllerDevUrl = "http://localhost:9804/user/";

    private readonly HttpClient _httpClient;

    private string CreateRemoveUserRequest(Guid projectId)
    {
      var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

      url.Add("projectId", projectId.ToString());

      return "remove?" + url.ToString();
    }

    public ProjectUserController(string accessToken)
    {
      _httpClient = new HttpClient();

      _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
      _httpClient.BaseAddress = new Uri(UserControllerDevUrl);
#else
      _httpClient.BaseAddress = new Uri(UserControllerProdUrl);
#endif
    }

    public Task<HttpResponseMessage> Create(CreateProjectUsersRequest request)
    {
      var httpContent = new StringContent(
        JsonConvert.SerializeObject(request),
        Encoding.UTF8,
        "application/json");

      return _httpClient.PostAsync("create", httpContent);
    }

    public Task<HttpResponseMessage> Remove(Guid projectId, List<Guid> usersIds)
    {
      var httpContent = new StringContent(
        JsonConvert.SerializeObject(usersIds),
        Encoding.UTF8,
        "application/json");

      return _httpClient.DeleteAsync(CreateRemoveUserRequest(projectId), httpContent);
    }
  }
}
