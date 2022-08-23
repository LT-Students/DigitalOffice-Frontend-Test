using LT.DigitalOffice.LoadTesting.Models.Users.Models;
using System;
using System.Collections.Generic;

namespace DigitalOffice.LoadTesting.Models.Users.Models
{
  public record UserInfo
  {
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsActive { get; set; }
    public PendingUserInfo PendingInfo { get; set; }
    public ImageInfo Avatar { get; set; }
    public IEnumerable<CommunicationInfo> Communications { get; set; }
  }
}
