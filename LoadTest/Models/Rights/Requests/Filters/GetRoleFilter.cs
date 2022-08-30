using System;

namespace LT.DigitalOffice.LoadTesting.Models.Rights.Requests.Filters
{
  public record GetRoleFilter
  {
    public Guid RoleId { get; set; }
    public string Locale { get; set; }
  }
}
