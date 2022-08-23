using System;
using System.Collections.Generic;

namespace DigitalOffice.LoadTesting.Models.Message.Models.Emails
{
    public record EmailTemplateInfo
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public List<EmailTemplateTextInfo> Texts { get; set; }
    }
}
