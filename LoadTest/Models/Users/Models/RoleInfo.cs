using System;
using System.Collections.Generic;

namespace DigitalOffice.LoadTesting.Models.Users.Models
{
  public record RoleInfo
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<int> RightsIds { get; set; }
  }
}
