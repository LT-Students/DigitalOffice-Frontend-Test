using DigitalOffice.LoadTesting.Models.Message.Enums;

namespace DigitalOffice.LoadTesting.Models.Message.Requests.ParseEntity
{
    public record AddKeywordRequest
    {
        public string Keyword { get; set; }
        public ServiceName ServiceName { get; set; }
        public string EntityName { get; set; }
        public string PropertyName { get; set; }
    }
}
