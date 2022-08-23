using DigitalOffice.LoadTesting.Models.Project.Enums;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.LoadTesting.Models.Project.Requests.User
{
  public record EditProjectUsersRoleRequest
  {
    public ProjectUserRoleType Role { get; set; }
    public List<Guid> UsersIds { get; set; }
  }
}
