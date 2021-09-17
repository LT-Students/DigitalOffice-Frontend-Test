using DigitalOffice.LoadTesting.Models.Users.Enums;
using System;

namespace DigitalOffice.LoadTesting.Models.Users.Requests.User.Communication
{
    public record EditCommunicationRequest
    {
        public CommunicationType Type { get; set; }
        public string Value { get; set; }
    }
}
