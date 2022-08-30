using DigitalOffice.LoadTesting.Models.Project.Enums;
using System;

namespace LT.DigitalOffice.LoadTesting.Models.Project.Models
{
  public record ProjectUserInfo
  {
    public Guid UserId { get; set; }
    public ProjectUserRoleType Role { get; set; }
  }
}
