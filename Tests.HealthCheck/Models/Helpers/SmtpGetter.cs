using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Tests.HealthCheck.Models.Helpers
{
    public class SmtpGetter
    {
        private readonly IRequestClient<IGetSmtpCredentialsRequest> _rcGetSmtp;
        private readonly ILogger<SmtpGetter> _logger;

        public SmtpGetter(
            IRequestClient<IGetSmtpCredentialsRequest> rcGetSmtp,
            ILogger<SmtpGetter> logger)
        {
            _rcGetSmtp = rcGetSmtp;
            _logger = logger;
        }

        public async Task<bool> GetSmtp()
        {
            try
            {
                IOperationResult<IGetSmtpCredentialsResponse> response = (await _rcGetSmtp.GetResponse<IOperationResult<IGetSmtpCredentialsResponse>>(
                  IGetSmtpCredentialsRequest.CreateObj())).Message;

                if (!response.IsSuccess)
                {
                    _logger.LogWarning("Can not get SmtpCredentials from Company.");
                    return false;
                }

                SmtpCredentials.Email = response.Body.Email;
                SmtpCredentials.EnableSsl = response.Body.EnableSsl;
                SmtpCredentials.Host = response.Body.Host;
                SmtpCredentials.Password = response.Body.Password;
                SmtpCredentials.Port = response.Body.Port;
            }
            catch
            {
                _logger.LogWarning("Can not get SmtpCredentials from Company.");
                return false;
            }

            return true;
        }

    }
}
