using LT.DigitalOffice.LoadTesting.Models.Department.Enums;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.LoadTesting.Models.Department.Requests.User
{
  public record EditDepartmentUserAssignmentRequest
  {
    public DepartmentUserAssignment Assignment { get; set; }
    public List<Guid> UsersIds { get; set; }
  }
}
