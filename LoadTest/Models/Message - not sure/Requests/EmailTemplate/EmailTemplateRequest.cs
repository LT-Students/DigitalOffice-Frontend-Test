using DigitalOffice.LoadTesting.Models.Message.Enums;
using DigitalOffice.LoadTesting.Models.Message.Requests.EmailTemplateText;
using System.Collections.Generic;

namespace DigitalOffice.LoadTesting.Models.Message.Requests.EmailTemplate
{
    public record EmailTemplateRequest
    {
        public string Name { get; set; }
        public EmailTemplateType Type { get; set; }
        public IEnumerable<EmailTemplateTextRequest> EmailTemplateTexts { get; set; }
    }
}
