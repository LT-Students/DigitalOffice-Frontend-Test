namespace DigitalOffice.LoadTesting.Models.Message.Requests.EmailTemplateText
{
    public record EditEmailTemplateTextRequest
    {
        public string Subject { get; set; }
        public string Text { get; set; }
        public string Language { get; set; }
    }
}
