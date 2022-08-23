using DigitalOffice.LoadTesting.Models.Project.Enums;
using DigitalOffice.LoadTesting.Models.Project.Models;
using System;

namespace LT.DigitalOffice.LoadTesting.Models.Project.Models
{
  public class UserInfo
  {
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public bool IsActive { get; set; }
    public ProjectUserRoleType Role { get; set; }
    public ImageInfo AvatarImage { get; set; }
    public PositionInfo Position { get; set; }
  }
}
