using System;

namespace DigitalOffice.LoadTesting.Models.Users.Models
{
  public record DepartmentInfo
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
  }
}
