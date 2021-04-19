using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using HealthChecks.UI.Core;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using Microsoft.Extensions.Options;
using Tests.HealthCheck.Models;

namespace Tests.HealthCheck
{
    public static class EmailSender
    {
        public static bool SendEmail(
            string[] emails,
            SmtpCredentialsOptions options,
            UIHealthReport errorReport)
        {
            var smtp = new SmtpClient(options.Host, options.Port)
            {
                Credentials = new NetworkCredential(options.Email, options.Password),
                EnableSsl = true
            };
            MailAddress from = new(options.Email);
            
            for (int i = 0; i < emails.Length; i++)
            {
                MailAddress to = new(emails[i]);

                var message = new MailMessage(from, to)
                {
                    Subject = "HealthCheck",
                    Body = $"{string.Join(", ", errorReport.Entries.Values.Select(x => x.Description))} is failed with error message"
                };

                smtp.Send(message);
            }
            return true;
        }
    }
}