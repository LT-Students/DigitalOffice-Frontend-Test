using DigitalOffice.LoadTesting.Models.Users.Enums;
using System;

namespace DigitalOffice.LoadTesting.Models.Users.Models
{
    public record CommunicationInfo
    {
        public Guid Id { get; set; }
        public CommunicationType Type { get; set; }
        public string Value { get; set; }
    }
}
