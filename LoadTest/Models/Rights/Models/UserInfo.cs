using System;

namespace DigitalOffice.LoadTesting.Models.Rights.Models
{
  public record UserInfo
  {
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
  }
}
