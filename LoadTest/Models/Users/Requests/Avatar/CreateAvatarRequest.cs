using System;

namespace LT.DigitalOffice.LoadTesting.Models.Users.Requests.Avatar
{
  public record CreateAvatarRequest
  {
    public Guid? UserId { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public string Extension { get; set; }
    public bool IsCurrentAvatar { get; set; }
  }
}
