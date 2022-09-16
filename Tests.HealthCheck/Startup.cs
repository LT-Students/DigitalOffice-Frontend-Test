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
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tests.HealthCheck.Models.Configurations;
using Tests.HealthCheck.Models.Helpers;

namespace Tests.HealthCheck
{
  public class Startup
  {
    public IConfiguration Configuration { get; }

    private readonly HealthCheckEndpointsConfig _healthCheckConfig;
    private readonly RabbitMqConfig _rabbitMqConfig;
    private readonly BaseServiceInfoConfig _serviceInfoConfig;
    private readonly ILogger<SmtpGetter> _logger;

    private static List<(string ServiceName, string Uri)> _servicesInfo;
    private static string[] _emails;
    private static int _interval;

    private void ConfigureHcEndpoints(Settings setup)
    {
      foreach ((string serviceName, string uri) in _servicesInfo)
      {
        setup.AddHealthCheckEndpoint(
          serviceName,
          uri);
      }
    }

    private (string username, string password) GetRabbitMqCredentials(
      RabbitMqConfig rabbitMqConfig,
      BaseServiceInfoConfig serviceInfoConfig)
    {
      string GetString(string envVar, string fromAppsettings, string generated, string fieldName)
      {
        string str = Environment.GetEnvironmentVariable(envVar);

        if (string.IsNullOrEmpty(str))
        {
          str = fromAppsettings ?? generated;
        }

        return str;
      }

      return (
        GetString("RabbitMqUsername", rabbitMqConfig.Username, $"{serviceInfoConfig.Name}_{serviceInfoConfig.Id}",
          "Username"),
        GetString("RabbitMqPassword", rabbitMqConfig.Password, serviceInfoConfig.Id, "Password"));
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

          _servicesInfo = _healthCheckConfig
            .GetType()
            .GetProperties()
            .Select(x => (ServiceName: x.Name, Uri: (string)x.GetValue(_healthCheckConfig)))
            .ToList();

          ConfigureHcEndpoints(setup);
        })
        .AddInMemoryStorage();

      (string username, string password) = GetRabbitMqCredentials(_rabbitMqConfig, _serviceInfoConfig);

      services.AddMassTransit(x =>
      {
        x.UsingRabbitMq((context, cfg) =>
        {
          cfg.Host(_rabbitMqConfig.Host, "/", host =>
          {
            host.Username(username);
            host.Password(password);
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

            //TODO: Remove
            var client = new HttpClient();
            client.Send(new HttpRequestMessage(HttpMethod.Get, "https://office.dev.ltdo.xyz/office/find"));

      IServiceProvider serviceProvider = app.ApplicationServices.GetRequiredService<IServiceProvider>();

      IServiceScope scope = app.ApplicationServices.CreateScope();

      IRequestClient<IGetSmtpCredentialsRequest> rcGetSmtpCredentials =
        serviceProvider.CreateRequestClient<IGetSmtpCredentialsRequest>(
          new Uri($"{_rabbitMqConfig.BaseUrl}/{_rabbitMqConfig.GetSmtpCredentialsEndpoint}"), default);

      SmtpGetter smtpGetter = new SmtpGetter(rcGetSmtpCredentials, _logger);

      if (smtpGetter.GetSmtp().Result)
      {
        Task.Run(() => ReportEmailSender.Start(_interval, _emails));
      }
    }
  }
}