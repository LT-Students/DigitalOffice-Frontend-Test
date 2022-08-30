using DigitalOffice.LoadTesting.Models.Message.Enums;

namespace DigitalOffice.LoadTesting.Models.Message.Requests.EmailTemplate
{
    public record EditEmailTemplateRequest
    {
        public string Name { get; set; }
        public EmailTemplateType Type { get; set; }
        public bool IsActive { get; set; }
    }
}
