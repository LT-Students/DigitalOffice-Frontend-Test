using DigitalOffice.LoadTesting.Models.Rights.Models;
using System.Collections.Generic;

namespace DigitalOffice.LoadTesting.Models.Rights.Responses
{
  public record RoleResponse
  {
    public RoleInfo Role { get; set; }
    public List<UserInfo> Users { get; set; }
  }
}
