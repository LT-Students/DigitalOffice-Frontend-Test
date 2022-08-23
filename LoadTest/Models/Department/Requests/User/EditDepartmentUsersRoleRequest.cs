using LT.DigitalOffice.LoadTesting.Models.Common.Enums;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.LoadTesting.Models.Department.Requests.User
{
  public record EditDepartmentUsersRoleRequest
  {
    public DepartmentUserRole Role { get; set; }
    public List<Guid> UsersIds { get; set; }
  }
}
