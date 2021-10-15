using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using MassTransit;
using System.Threading.Tasks;

namespace Tests.HealthCheck.Models.Helpers
{
    public class SmtpGetter
    {
        private readonly IRequestClient<IGetSmtpCredentialsRequest> _rcGetSmtp;

        public SmtpGetter(
            IRequestClient<IGetSmtpCredentialsRequest> rcGetSmtp
        )
        {
            _rcGetSmtp = rcGetSmtp;
        }

        public async Task<bool> GetSmtp()
        {
            try
            {
                IGetSmtpCredentialsResponse response = (await _rcGetSmtp.GetResponse<IOperationResult<IGetSmtpCredentialsResponse>>(
                  IGetSmtpCredentialsRequest.CreateObj())).Message.Body;

                SmtpCredentials.Email = response.Email;
                SmtpCredentials.EnableSsl = response.EnableSsl;
                SmtpCredentials.Host = response.Host;
                SmtpCredentials.Password = response.Password;
                SmtpCredentials.Port = response.Port;
            }
            catch
            {
                return false;
            }

            return true;
        }

    }
}
