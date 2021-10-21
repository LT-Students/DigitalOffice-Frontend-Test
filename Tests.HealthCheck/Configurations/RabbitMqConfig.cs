using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Requests.Message;

namespace LT.DigitalOffice.Tests.Models.Dto.Configurations
{
    public class RabbitMqConfig : BaseRabbitMqConfig
    {
        [AutoInjectRequest(typeof(IGetSmtpCredentialsRequest))]
        public string GetSmtpCredentialsEndpoint { get; set; }

        [AutoInjectRequest(typeof(ISendEmailRequest))]
        public string SendEmailEndpoint { get; set; }
    }
}
