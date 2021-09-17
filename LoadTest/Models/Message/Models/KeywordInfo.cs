using DigitalOffice.LoadTesting.Models.Message.Enums;
using System;

namespace DigitalOffice.LoadTesting.Models.Message.Models
{
    public record KeywordInfo
    {
        public Guid Id { get; set; }
        public string Keyword { get; set; }
        public ServiceName ServiceName { get; set; }
        public string EntityName { get; set; }
        public string PropertyName { get; set; }
    }
}
