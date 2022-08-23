using System;

namespace LT.DigitalOffice.LoadTesting.Models.Users.Models
{
  public record PendingUserInfo
  {
    public Guid InvitationCommunicationId { get; set; }
  }
}
