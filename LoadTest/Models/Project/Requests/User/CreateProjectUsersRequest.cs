using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.LoadTesting.Models.Project.Requests.User
{
  public record CreateProjectUsersRequest
  {
    public Guid ProjectId { get; set; }
    public List<UserRequest> Users { get; set; }
  }
}
