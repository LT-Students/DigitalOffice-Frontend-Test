using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using HealthChecks.UI.Core;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Helpers;

namespace Tests.HealthCheck
{
    public static class ReportEmailSender
    {
        private static readonly List<string> _reports = new();
        private static TimeSpan _interval;
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
            _interval = TimeSpan.FromMinutes(interval);

            while (true)
            {
                Task.Delay(_interval).Wait();
                SendEmails();
            }
        }

        public static void SendEmails()
        {
            var smtp = new SmtpClient(
                GetEnvironmentVariableHelper.Get(ConstStrings.Host),
                int.Parse(GetEnvironmentVariableHelper.Get(ConstStrings.Port)))
            {
                Credentials = new NetworkCredential(
                    GetEnvironmentVariableHelper.Get(ConstStrings.Email),
                    GetEnvironmentVariableHelper.Get(ConstStrings.Password)),
                EnableSsl = true
            };

            MailAddress from = new(GetEnvironmentVariableHelper.Get(ConstStrings.Email));

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