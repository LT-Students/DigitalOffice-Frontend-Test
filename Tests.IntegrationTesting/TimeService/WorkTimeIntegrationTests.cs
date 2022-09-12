using DigitalOffice.LoadTesting.Models.Responses.Templates;
using DigitalOffice.LoadTesting.Models.Time.Responses;
using LT.DigitalOffice.LoadTesting.Models.Time.Requests;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DigitalOffice.LoadTesting.Models.Time.Requests;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json.Serialization;
using Tests.IntegrationTesting.Helpers;

namespace Tests.IntegrationTesting.TimeService
{
  internal class WorkTimeIntegrationTests
  {
    private const string TimeServiceWorkTimeUrl = "https://time.dev.ltdo.xyz/worktime/";
    private const string CreateUrl = $"{TimeServiceWorkTimeUrl}create";
    private const string FindUrl = $"{TimeServiceWorkTimeUrl}find";
    private const string EditUrl = $"{TimeServiceWorkTimeUrl}edit";
    private readonly Guid _userId = Guid.Parse("a86612a3-fa5f-4cbe-a6ce-7d0a18556d12");
    private Guid? _workTimeId;
    private string _token;

    private HttpClient _httpClient;

    //[OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
      _httpClient = new HttpClient();
      _token = await TokenHelper.GetToken();
      _httpClient.DefaultRequestHeaders.Add("token", _token);
    }

    //[Test]
    public async Task CreateWorkTimeSuccessfully()
    {
      CreateWorkTimeRequest request = new()
      {
        Hours = 228,
        Description = "WorkTimeFromIntegrationTests",
        Year = DateTime.Now.Year,
        Month = DateTime.Now.Month
      };
      string json = JsonConvert.SerializeObject(request);
      StringContent data = new(json, Encoding.UTF8, "application/json");

      HttpResponseMessage response = await _httpClient.PostAsync(CreateUrl, data);

      string result = await response.Content.ReadAsStringAsync();

      Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }

    //[Test]
    public async Task FindWorkTimeSuccessfully()
    {
      string filterParams = $"?userid={_userId}";

      HttpResponseMessage response = await _httpClient.GetAsync(string.Concat(FindUrl, filterParams));

      string result = await response.Content.ReadAsStringAsync();

      FindResultResponse<WorkTimeResponse> data = JsonConvert.DeserializeObject<FindResultResponse<WorkTimeResponse>>(result);

      List<WorkTimeResponse> workTimes = data?.Body;
      _workTimeId = workTimes?.FirstOrDefault()!.WorkTime.Id;

      Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
      Assert.That(workTimes?.Count, Is.EqualTo(1));
    }

    //[Test]
    public async Task EditWorkTimeSuccessfully()
    {
      string value = "Edited Description";
      JsonPatchDocument<EditWorkTimeRequest> patchRequest = new(new List<Operation<EditWorkTimeRequest>>
      {
        new(
          "replace",
          $"/{nameof(EditWorkTimeRequest.Description)}",
          "",
          value)
      }, new CamelCasePropertyNamesContractResolver());

      string json = JsonConvert.SerializeObject(patchRequest);
      StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
      string queryParams = $"?worktimeid={_workTimeId}";

      HttpResponseMessage response = await _httpClient.PatchAsync(string.Concat(EditUrl, queryParams), data);
      string responseData = await response.Content.ReadAsStringAsync();

      OperationResultResponse<bool> result = JsonConvert.DeserializeObject<OperationResultResponse<bool>>(responseData);

      Assert.That(result.Body, Is.True);
    }
  }
}
