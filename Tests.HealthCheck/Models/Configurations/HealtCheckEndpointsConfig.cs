using LT.DigitalOffice.Kernel.Configurations;

namespace Tests.HealthCheck.Models.Configurations
{
    public class HealthCheckEndpointsConfig
    {
        public const string SectionName = "HealthCheckEndpoints";
        
        public string UserHealthCheckEndpoint { get; set; }
        public string ProjectHealthCheckEndpoint { get; set; }
        public string TimeHealthCheckEndpoint { get; set; }
        public string MessageHealthCheckEndpoint { get; set; }
        public string RightsHealthCheckEndpoint { get; set; }
        public string FileHealthCheckEndpoint { get; set; }
        public string CompanyHealthCheckEndpoint { get; set; }
        public string AuthHealthCheckEndpoint { get; set; }
    }
}