using DigitalOffice.LoadTesting.Models.Message.Models.Emails;
using System.Collections.Generic;

namespace DigitalOffice.LoadTesting.Models.Message.Responses
{
    public record UnsentEmailsResponse
    {
        public int TotalCount { get; set; }
        public List<UnsentEmailInfo> Emails { get; set; } = new();
    }
}
