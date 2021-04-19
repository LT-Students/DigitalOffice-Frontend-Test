using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using HealthChecks.UI.Core;
using Tests.HealthCheck.Models;

namespace Tests.HealthCheck
{
    public static class EmailSender
    {
        private static readonly List<string> _reports = new();
        private static TimeSpan _interval;
        private static string[] _emails;
        private static SmtpCredentialsOptions _options;
        
        public static void AddReport(UIHealthReport newReport)
        {
            _reports.Add(string.Join(
                ", ", 
                newReport.Entries.Values.Select(x => x.Description)));
        }

        public static void Start(int interval, string[] emails, SmtpCredentialsOptions options)
        {
            _emails = emails;
            _options = options;
            _interval = TimeSpan.FromMinutes(interval);
            
            while (true)
            {
                Task.Delay(_interval).Wait();
                SendEmails();
            }
        }
        
        public static void SendEmails()
        { 
            var smtp = new SmtpClient(_options.Host, _options.Port)
            {
                Credentials = new NetworkCredential(_options.Email, _options.Password),
                EnableSsl = true
            };
            MailAddress from = new(_options.Email);

            for (int i = 0; i < _emails.Length; i++)
            {
                MailAddress to = new(_emails[i]);

                var message = new MailMessage(from, to)
                {
                    Subject = "HealthCheck",
                    Body =
                        $"{string.Join("; ", _reports)} is failed with error message"
                };

                smtp.Send(message);
            }
        }
    }
}