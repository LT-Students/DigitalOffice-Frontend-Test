using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using HealthChecks.UI.Core;
using Tests.HealthCheck.Models.Helpers;

namespace Tests.HealthCheck
{
    public static class ReportEmailSender
    {
        private static readonly List<string> _reports = new();
        private static List<string> _emails;

        public static void AddReport(UIHealthReport newReport)
        {
            _reports.Add(string.Join(
                ", ",
                newReport.Entries.Values.Select(x => x.Description)));
        }

        public static void Start(int interval, string[] emails)
        {
            _emails = emails.ToList();

            while (true)
            {
                Task.Delay(interval).Wait();
                SendEmails();
            }
        }

        public static void SendEmails()
        {
            SmtpClient smtp = new SmtpClient(
                SmtpCredentials.Host,
                SmtpCredentials.Port)
            {
                Credentials = new NetworkCredential(
                    SmtpCredentials.Email,
                    SmtpCredentials.Password),
                EnableSsl = SmtpCredentials.EnableSsl
            };

            MailAddress from = new(SmtpCredentials.Email);

            for (int i = 0; i < _emails.Count; i++)
            {
                MailAddress to = new(_emails[i]);

                MailMessage message = new MailMessage(from, to)
                {
                    From = new MailAddress(SmtpCredentials.Email),
                    Subject = "HealthCheck",
                    Body =
                        $"Failed with error messages: {Environment.NewLine}{string.Join("; ", _reports)}."
                };

                smtp.Send(message);
            }
        }
    }
}