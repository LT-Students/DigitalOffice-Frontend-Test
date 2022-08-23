using DigitalOffice.LoadTesting.Models.Users.Models;
using LT.DigitalOffice.LoadTesting.Models.Common.Enums;

namespace LT.DigitalOffice.LoadTesting.Models.Users.Models
{
  public record DepartmentUserInfo
  {
    public DepartmentInfo Department { get; set; }
    public DepartmentUserRole Role { get; set; }
  }
}
