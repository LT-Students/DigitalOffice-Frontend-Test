using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.LoadTesting.Models.Department.Requests.User
{
  public record CreateDepartmentUsersRequest
  {
    public Guid DepartmentId { get; set; }
    public List<CreateUserRequest> Users { get; set; }
  }
}
