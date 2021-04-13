using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LT.DigitalOffice.Kernel.Configurations;
using Tests.HealthCheck.Models.Configurations;

//TODO fill appsettings.
namespace Tests.HealthCheck
{
    public class Startup
    {
        private readonly RabbitMqConfig _rabbitMqConfig;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _rabbitMqConfig = Configuration
                .GetSection(BaseRabbitMqConfig.SectionName)
                .Get<RabbitMqConfig>();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services
                .AddHealthChecksUI(setupSettings: setup =>
                {
                    setup.AddHealthCheckEndpoint(
                        nameof(_rabbitMqConfig.UserHealthCheckEndpoint),
                        _rabbitMqConfig.UserHealthCheckEndpoint);

                    setup.AddHealthCheckEndpoint(
                        nameof(_rabbitMqConfig.ProjectHealthCheckEndpoint),
                        _rabbitMqConfig.ProjectHealthCheckEndpoint);

                    setup.AddHealthCheckEndpoint(
                        nameof(_rabbitMqConfig.CompanyHealthCheckEndpoint),
                        _rabbitMqConfig.CompanyHealthCheckEndpoint);
                    
                    setup.AddHealthCheckEndpoint(
                        nameof(_rabbitMqConfig.CompanyHealthCheckEndpoint),
                        _rabbitMqConfig.CompanyHealthCheckEndpoint);
                    
                    setup.AddHealthCheckEndpoint(
                        nameof(_rabbitMqConfig.CompanyHealthCheckEndpoint),
                        _rabbitMqConfig.CompanyHealthCheckEndpoint);
                    
                    setup.AddHealthCheckEndpoint(
                        nameof(_rabbitMqConfig.CompanyHealthCheckEndpoint),
                        _rabbitMqConfig.CompanyHealthCheckEndpoint);
                    
                    setup.AddHealthCheckEndpoint(
                        nameof(_rabbitMqConfig.CompanyHealthCheckEndpoint),
                        _rabbitMqConfig.CompanyHealthCheckEndpoint);
                    
                    setup.AddHealthCheckEndpoint(
                        nameof(_rabbitMqConfig.CompanyHealthCheckEndpoint),
                        _rabbitMqConfig.CompanyHealthCheckEndpoint);
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