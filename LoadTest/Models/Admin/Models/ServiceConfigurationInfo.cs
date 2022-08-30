using System;

namespace LT.DigitalOffice.LoadTesting.Models.Admin.Models
{
  public record ServiceConfigurationInfo
  {
    public Guid Id { get; set; }
    public string ServiceName { get; set; }
    public bool IsActive { get; set; }
  }
}
