using DigitalOffice.LoadTesting.Models;
using NBomber.Contracts;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace LT.DigitalOffice.LoadTesting.LoadTests
{
  public abstract class BaseScenarioCreatorAsync
  {
    protected readonly string _path;
    protected readonly int _rate;
    protected readonly TimeSpan _during;
    protected readonly TimeSpan _warmUpTime;
    protected readonly TimeSpan _responseTimeout;
    protected readonly HttpClient _httpClient;

    protected Response CreateResponse(HttpResponseMessage message, HttpStatusCode expected)
    {
      HttpStatusCode code = message.StatusCode;

      return expected == code
        ? Response.Ok(statusCode: (int)code)
        : Response.Fail(statusCode: (int)code);
    }

    public BaseScenarioCreatorAsync(ScenarioStartSettings settings)
    {
      _httpClient = new HttpClient();

      _path = settings.Path;
      _rate = settings.Rate;
      _during = settings.During;
      _warmUpTime = settings.WarmUpTime;
      _responseTimeout = settings.ResponseTimeout;
      _httpClient.DefaultRequestHeaders.Add("token", settings.Token);
    }

    public abstract Task Run();
  }
}
