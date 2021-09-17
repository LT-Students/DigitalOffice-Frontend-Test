using DigitalOffice.LoadTesting.Models;
using NBomber.Contracts;
using System;
using System.Net;
using System.Net.Http;

namespace DigitalOffice.LoadTesting.Services
{
    public abstract class BaseScenarioCreator
    {
        protected readonly string _path;
        protected readonly int _rate;
        protected readonly TimeSpan _during;
        protected readonly TimeSpan _warmUpTime;
        protected readonly HttpClient _httpClient;

        protected Response CreateResponse(HttpResponseMessage message, HttpStatusCode expected)
        {
            HttpStatusCode code = message.StatusCode;

            return expected == code
                ? Response.Ok(statusCode: (int)code)
                : Response.Fail(statusCode: (int)code);
        }

        public BaseScenarioCreator(ScenarioStartSettings settings)
        {
            _httpClient = new HttpClient();

            _path = settings.Path;
            _rate = settings.Rate;
            _during = settings.During;
            _warmUpTime = settings.WarmUpTime;
            _httpClient.DefaultRequestHeaders.Add("token", settings.Token);
        }

        public abstract void Run();
    }
}
