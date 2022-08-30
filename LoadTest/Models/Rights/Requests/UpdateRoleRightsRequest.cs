using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.LoadTesting.Models.Rights.Requests
{
  public class UpdateRoleRightsRequest
  {
    public Guid RoleId { get; set; }
    public List<int> Rights { get; set; }
  }
}
