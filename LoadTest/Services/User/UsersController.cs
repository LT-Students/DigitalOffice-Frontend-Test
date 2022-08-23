using DigitalOffice.LoadTesting.Helpers;
using DigitalOffice.LoadTesting.Models.Users.Requests.User;
using DigitalOffice.LoadTesting.Models.Users.Requests.User.Filters;
using LT.DigitalOffice.LoadTesting.Models.Users.Requests.User.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.User
{
  public class UsersController
  {
    private const string UsersControllerProdUrl = "https://user.ltdo.xyz/user/";
    private const string UsersControllerDevUrl = "http://localhost:9802/user/";

    private readonly HttpClient _httpClient;

    private string CreateGetUserRequest(GetUserFilter filter)
    {
      NameValueCollection url = System.Web.HttpUtility.ParseQueryString(string.Empty);

      url.Add("userId", filter.UserId.ToString());
      url.Add("email", filter.Email?.ToString());
      url.Add("login", filter.Login?.ToString());
      url.Add("includeAchievements", filter.IncludeAchievements.ToString());
      url.Add("includeCurrentAvatar", filter.IncludeCurrentAvatar.ToString());
      url.Add("includeAvatars", filter.IncludeAvatars.ToString());
      url.Add("includeCertificates", filter.IncludeCertificates.ToString());
      url.Add("includeCommunications", filter.IncludeCommunications.ToString());
      url.Add("includeCompany", filter.IncludeCompany.ToString());
      url.Add("includeDepartment", filter.IncludeDepartment.ToString());
      url.Add("includeEducations", filter.IncludeEducations.ToString());
      url.Add("includeOffice", filter.IncludeOffice.ToString());
      url.Add("includePosition", filter.IncludePosition.ToString());
      url.Add("includeProjects", filter.IncludeProjects.ToString());
      url.Add("includeRole", filter.IncludeRole.ToString());
      url.Add("includeSkills", filter.IncludeSkills.ToString());
      url.Add("locale", filter.Locale?.ToString());

      return "get?" + url.ToString();
    }

    private string CreateFindUsersRequest(FindUsersFilter filter)
    {
      NameValueCollection url = System.Web.HttpUtility.ParseQueryString(string.Empty);

      url.Add("isAscendingSort", filter.IsAscendingSort.ToString());
      url.Add("fullNameIncludeSubstring", filter.FullNameIncludeSubstring?.ToString());
      url.Add("isActive", filter.IsActive.ToString());
      url.Add("includeCurrentAvatar", filter.IncludeCurrentAvatar.ToString());
      url.Add("includeCommunications", filter.IncludeCommunications.ToString());
      url.Add("skipCount", filter.SkipCount.ToString());
      url.Add("takeCount", filter.TakeCount.ToString());

      return "find?" + url.ToString();
    }

    private string CreateEditUsersRequest(Guid userId)
    {
      var url = System.Web.HttpUtility.ParseQueryString(string.Empty);

      url.Add("userId", userId.ToString());

      return "edit?" + url.ToString();
    }

    public UsersController(string accessToken)
    {
      _httpClient = new HttpClient();

      _httpClient.DefaultRequestHeaders.Add("token", accessToken);

#if DEBUG
      _httpClient.BaseAddress = new Uri(UsersControllerDevUrl);
#else
      _httpClient.BaseAddress = new Uri(UsersControllerProdUrl);
#endif
    }

    public Task<HttpResponseMessage> Get(Guid userId)
    {
      return _httpClient.GetAsync(CreateGetUserRequest(new GetUserFilter { UserId = userId }));
    }

    public Task<HttpResponseMessage> Find(
      int skipCount = 0,
      int takeCount = int.MaxValue,
      bool? isAscendingSort = true,
      bool? isActive = null)
    {
      return _httpClient.GetAsync(
        CreateFindUsersRequest(
          new FindUsersFilter
          {
            TakeCount = takeCount,
            SkipCount = skipCount,
            IsAscendingSort = isAscendingSort,
            IsActive = isActive
          }));
    }

    public Task<HttpResponseMessage> Create(CreateUserRequest request)
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
