using System;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LT.DigitalOffice.Kernel.Configurations;
using Tests.HealthCheck.Models;
using Tests.HealthCheck.Models.Configurations;

//TODO add reflex to config
namespace Tests.HealthCheck
{
    public class Startup
    {
        private string GetTokenFromAuth()
        {
            AuthLoginConfig authLoginConfig = Configuration
                .GetSection(AuthLoginConfig.SectionName)
                .Get<AuthLoginConfig>();

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest
                .Create(authLoginConfig.UriString);

            string login = "admin";
            string password = "%4fgT1_3ioR";
            string stringData = $"{{ \"LoginData\": \"{login}\",\"Password\": \"{password}\" }}";
            
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
            var token = response.Split("\"token\":")[^1].Trim('}').Trim('\"');
            
            return token;
        }
        
        private readonly HealthCheckEndpointsConfig _healthCheckConfig;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _healthCheckConfig = Configuration
                .GetSection(HealthCheckEndpointsConfig.SectionName)
                .Get<HealthCheckEndpointsConfig>();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string token = GetTokenFromAuth();

            services.AddControllers();

            services
                .AddHealthChecksUI(setupSettings: setup =>
                {
                    string evaluationString = Environment.GetEnvironmentVariable("EvaluationTimeInSeconds");
                    if (!string.IsNullOrEmpty(evaluationString) 
                        && int.TryParse(evaluationString, out int evaluationSeconds))
                    {
                        setup.SetEvaluationTimeInSeconds(evaluationSeconds);
                    }
                    
                    setup.ConfigureApiEndpointHttpclient((sp, client) =>
                    {
                        client.DefaultRequestHeaders.Add("token", token);
                    });

                    setup.AddHealthCheckEndpoint(
                        nameof(_healthCheckConfig.UserHealthCheckEndpoint),
                        _healthCheckConfig.UserHealthCheckEndpoint);

                    setup.AddHealthCheckEndpoint(
                        nameof(_healthCheckConfig.ProjectHealthCheckEndpoint),
                        _healthCheckConfig.ProjectHealthCheckEndpoint);

                    setup.AddHealthCheckEndpoint(
                        nameof(_healthCheckConfig.CompanyHealthCheckEndpoint),
                        _healthCheckConfig.CompanyHealthCheckEndpoint);

                    setup.AddHealthCheckEndpoint(
                        nameof(_healthCheckConfig.CompanyHealthCheckEndpoint),
                        _healthCheckConfig.CompanyHealthCheckEndpoint);

                    setup.AddHealthCheckEndpoint(
                        nameof(_healthCheckConfig.CompanyHealthCheckEndpoint),
                        _healthCheckConfig.CompanyHealthCheckEndpoint);

                    setup.AddHealthCheckEndpoint(
                        nameof(_healthCheckConfig.CompanyHealthCheckEndpoint),
                        _healthCheckConfig.CompanyHealthCheckEndpoint);

                    setup.AddHealthCheckEndpoint(
                        nameof(_healthCheckConfig.CompanyHealthCheckEndpoint),
                        _healthCheckConfig.CompanyHealthCheckEndpoint);

                    setup.AddHealthCheckEndpoint(
                        nameof(_healthCheckConfig.CompanyHealthCheckEndpoint),
                        _healthCheckConfig.CompanyHealthCheckEndpoint);
                })
                .AddInMemoryStorage();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
#if RELEASE
            app.UseHttpsRedirection();
#endif

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecksUI();
            });
        }
    }
}