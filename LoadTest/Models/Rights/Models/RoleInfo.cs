using DigitalOffice.LoadTesting.Models.Rights.Responses;
using System;
using System.Collections.Generic;

namespace DigitalOffice.LoadTesting.Models.Rights.Models
{
    public record RoleInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public IEnumerable<RightResponse> Rights { get; set; }
        public IEnumerable<UserInfo> Users { get; set; }
    }
}
