using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Models.Broker.Requests.Company;

namespace LT.DigitalOffice.Tests.Models.Dto.Configurations
{
  public class RabbitMqConfig : BaseRabbitMqConfig
  {
    [AutoInjectRequest(typeof(IGetSmtpCredentialsRequest))]
    public string GetSmtpCredentialsEndpoint { get; set; }
  }
}
