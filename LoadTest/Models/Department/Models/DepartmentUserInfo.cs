using LT.DigitalOffice.LoadTesting.Models.Common.Enums;
using LT.DigitalOffice.LoadTesting.Models.Department.Enums;
using System;

namespace LT.DigitalOffice.LoadTesting.Models.Department.Models
{
  public record DepartmentUserInfo
  {
    public Guid UserId { get; set; }
    public DepartmentUserRole Role { get; set; }
    public DepartmentUserAssignment Assignment { get; set; }
  }
}
