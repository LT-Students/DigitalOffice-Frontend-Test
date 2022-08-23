using DigitalOffice.LoadTesting.Models.Users.Enums;
using System;

namespace LT.DigitalOffice.LoadTesting.Models.Users.Requests.Communication
{
  public record CreateCommunicationRequest
  {
    public Guid? UserId { get; set; }
    public CommunicationType Type { get; set; } = CommunicationType.Email;
    public string Value { get; set; }
  }
}
