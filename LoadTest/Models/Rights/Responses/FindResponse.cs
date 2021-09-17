using DigitalOffice.LoadTesting.Models.Rights.Models;
using System.Collections.Generic;

namespace DigitalOffice.LoadTesting.Models.Rights.Responses
{
    public record FindResponse
    {
        public int TotalCount { get; set; }
        public List<RoleInfo> Roles { get; set; } = new();
        public List<string> Errors { get; set; } = new();
    }
}
