using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tests.HealthCheck.Models.Configurations;

namespace LT.DigitalOffice.Tests.HealthCheck.Models.Helpers
{
    public class CustomHttpClientHandler : HttpClientHandler
    {
        private readonly AuthLoginConfig _authLoginConfig;
        static (string value, DateTime recievedAtUtc) token = new();

        public CustomHttpClientHandler(
            AuthLoginConfig authLoginConfig)
        {
            _authLoginConfig = authLoginConfig;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var uri = request.RequestUri.AbsoluteUri;

            if (uri.EndsWith("/hc"))
            {
                AddToken(request);
            }

            return base.SendAsync(request, cancellationToken);
        }

        private void AddToken(HttpRequestMessage request)
        {
            if ((DateTime.UtcNow - token.recievedAtUtc).TotalMinutes < 110)
            {
                request.Headers.Add("token", token.value);
            }
            else
            {
                token.recievedAtUtc = DateTime.UtcNow;
                token.value = GetTokenHelper.GetToken(_authLoginConfig);

                request.Headers.Add("token", token.value);
            }
        }
    }
}
