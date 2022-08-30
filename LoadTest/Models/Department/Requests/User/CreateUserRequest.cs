using LT.DigitalOffice.LoadTesting.Models.Common.Enums;
using LT.DigitalOffice.LoadTesting.Models.Department.Enums;
using System;

namespace LT.DigitalOffice.LoadTesting.Models.Department.Requests.User
{
  public record CreateUserRequest
  {
    public Guid UserId { get; set; }
    public DepartmentUserRole Role { get; set; }
    public DepartmentUserAssignment Assignment { get; set; }
  }
}
