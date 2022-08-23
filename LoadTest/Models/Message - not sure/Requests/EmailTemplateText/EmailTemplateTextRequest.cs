using System;

namespace DigitalOffice.LoadTesting.Models.Message.Requests.EmailTemplateText
{
    public record EmailTemplateTextRequest
    {
        public Guid? EmailTemplateId { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public string Language { get; set; }
    }
}
