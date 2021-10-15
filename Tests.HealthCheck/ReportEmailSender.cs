using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using HealthChecks.UI.Core;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Helpers;
using Tests.HealthCheck.Models.Helpers;

namespace Tests.HealthCheck
{
    public static class ReportEmailSender
    {
        private static readonly List<string> _reports = new();
        private static TimeSpan _interval;
        private static List<string> _emails = new();

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

                var message = new MailMessage(from, to)
                {
                    Subject = "HealthCheck",
                    Body =
                        $"Failed with error messages: {Environment.NewLine}{string.Join("; ", _reports)}."
                };

                try
                {
                    smtp.Send(message);
                    _emails.RemoveAt(i);
                    i--;
                }
                catch (Exception )
                {

                }
            }
        }
    }
}