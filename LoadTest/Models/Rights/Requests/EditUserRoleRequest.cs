using System;

namespace LT.DigitalOffice.LoadTesting.Models.Rights.Requests
{
  public record EditUserRoleRequest
  {
    public Guid UserId { get; set; }
    public Guid? RoleId { get; set; }
  }
}
