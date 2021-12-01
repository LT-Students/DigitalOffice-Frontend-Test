using HealthChecks.UI.Configuration;
using HealthChecks.UI.Core;
using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;
using LT.DigitalOffice.Kernel.BrokerSupport.Extensions;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Tests.Models.Dto.Configurations;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tests.HealthCheck.Models.Configurations;
using Tests.HealthCheck.Models.Helpers;

namespace Tests.HealthCheck
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private readonly HealthCheckEndpointsConfig _healthCheckConfig;
        private static List<(string ServiceName, string Uri)> _servicesInfo;
        private static string[] _emails;
        private static int _interval;
        private readonly RabbitMqConfig _rabbitMqConfig;
        private readonly BaseServiceInfoConfig _serviceInfoConfig;
        private readonly ILogger<SmtpGetter> _logger;

        private void ConfigureHcEndpoints(Settings setup)
        {
            foreach ((string serviceName, string uri) in _servicesInfo)
            {
                setup.AddHealthCheckEndpoint(
                    serviceName,
                    uri);
            }
        }

        private string GetTokenFromAuthService()
        {
            AuthLoginConfig authLoginConfig = Configuration
                .GetSection(AuthLoginConfig.SectionName)
                .Get<AuthLoginConfig>();

            HttpWebRequest httpRequest = (HttpWebRequest) WebRequest
                .Create(authLoginConfig.UriString);

            string stringData =
                $"{{ \"LoginData\": \"{authLoginConfig.Login}\",\"Password\": \"{authLoginConfig.Password}\" }}";

            byte[] data = Encoding.Default.GetBytes(stringData);

            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json; charset=utf-8";
            httpRequest.ContentLength = data.Length;

            using Stream newStream = httpRequest.GetRequestStream();
            newStream.Write(data, 0, data.Length);

            using HttpWebResponse httpResponse = (HttpWebResponse) httpRequest.GetResponse();
            using Stream stream = httpResponse.GetResponseStream();
            using StreamReader reader = new StreamReader(stream);

            string response = reader.ReadToEnd();
            string[] separators = { "accessToken\":\"", "\",\"refreshToken" };
            string token = response.Split(separators, StringSplitOptions.TrimEntries)[1];

            return token;
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _healthCheckConfig = Configuration
                .GetSection(HealthCheckEndpointsConfig.SectionName)
                .Get<HealthCheckEndpointsConfig>();

            _rabbitMqConfig = Configuration
                .GetSection(BaseRabbitMqConfig.SectionName)
                .Get<RabbitMqConfig>();

            _serviceInfoConfig = Configuration
                .GetSection(BaseServiceInfoConfig.SectionName)
                .Get<BaseServiceInfoConfig>();

            _emails = Configuration
                .GetSection("SendEmailList")
                .Get<string[]>();

            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            _logger = loggerFactory.CreateLogger<SmtpGetter>();

            if (!int.TryParse(Environment.GetEnvironmentVariable("SendIntervalInMinutes"), out _interval))
            {
                _interval = Configuration.GetSection("SendIntervalInMinutes").Get<int>();
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string token = GetTokenFromAuthService();

            services.AddControllers();

            services
                .AddHealthChecksUI(setupSettings: setup =>
                {
                    setup
                        .AddWebhookNotification("", uri: "",
                            payload: "{ message: \"Webhook report for [[LIVENESS]]: [[FAILURE]] - Description: [[DESCRIPTIONS]]\"}",
                            restorePayload: "{ message: \"[[LIVENESS]] is back to life\"}",
                            customDescriptionFunc: report =>
                            {
                                IEnumerable<KeyValuePair<string, UIHealthReportEntry>> failing = report.Entries
                                    .Where(e => e.Value.Status != UIHealthStatus.Healthy);

                                ReportEmailSender.AddReport(report);

                                return $"{failing.Count()} healthchecks are failing";
                            });

                    string evaluationTimeString = Environment.GetEnvironmentVariable("EvaluationTimeInSeconds");
                    if (!string.IsNullOrEmpty(evaluationTimeString)
                        && int.TryParse(evaluationTimeString, out int evaluationTimeSeconds))
                    {
                        setup.SetEvaluationTimeInSeconds(evaluationTimeSeconds);
                    }

                    setup.ConfigureApiEndpointHttpclient((sp, client) =>
                    {
                        client.DefaultRequestHeaders.Add("token", token);
                    });

                    _servicesInfo = _healthCheckConfig
                        .GetType()
                        .GetProperties()
                        .Select(x => (ServiceName: x.Name, Uri: (string) x.GetValue(_healthCheckConfig)))
                        .ToList();

                    ConfigureHcEndpoints(setup);
                })
                .AddInMemoryStorage();

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(_rabbitMqConfig.Host, "/", host =>
                    {
                        host.Username($"{_serviceInfoConfig.Name}_{_serviceInfoConfig.Id}");
                        host.Password(_serviceInfoConfig.Id);
                    });
                });

                x.AddRequestClients(_rabbitMqConfig);
            });

            services.AddMassTransitHostedService();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecksUI();
            });

            IServiceProvider serviceProvider = app.ApplicationServices.GetRequiredService<IServiceProvider>();

            IServiceScope scope = app.ApplicationServices.CreateScope();

            IRequestClient<IGetSmtpCredentialsRequest> rcGetSmtpCredentials = serviceProvider.CreateRequestClient<IGetSmtpCredentialsRequest>(
                new Uri($"{_rabbitMqConfig.BaseUrl}/{_rabbitMqConfig.GetSmtpCredentialsEndpoint}"), default);

            SmtpGetter smtpGetter = new SmtpGetter(rcGetSmtpCredentials, _logger);

            if (smtpGetter.GetSmtp().Result)
            {
                Task.Run(() => ReportEmailSender.Start(_interval, _emails));
            }
        }
    }
}