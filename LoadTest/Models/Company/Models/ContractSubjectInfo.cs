using System;

namespace DigitalOffice.LoadTesting.Models.Company.Models
{
  public record ContractSubjectInfo
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
  }
}
