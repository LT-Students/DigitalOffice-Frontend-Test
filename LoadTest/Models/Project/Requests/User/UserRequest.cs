using DigitalOffice.LoadTesting.Models.Project.Enums;
using System;

namespace LT.DigitalOffice.LoadTesting.Models.Project.Requests.User
{
  public record UserRequest
  {
    public Guid UserId { get; set; }
    public ProjectUserRoleType Role { get; set; }
  }
}
