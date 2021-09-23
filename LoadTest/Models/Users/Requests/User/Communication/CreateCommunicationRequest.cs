using DigitalOffice.LoadTesting.Models.Users.Enums;
using System;

namespace DigitalOffice.LoadTesting.Models.Users.Requests.User.Communication
{
    public record CreateCommunicationRequest
    {
        public Guid? UserId { get; set; }
        public CommunicationType Type { get; set; }
        public string Value { get; set; }
    }
}
