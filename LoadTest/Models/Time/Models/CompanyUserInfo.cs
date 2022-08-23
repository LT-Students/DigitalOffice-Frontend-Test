using LT.DigitalOffice.LoadTesting.Models.Common;
using System;

namespace LT.DigitalOffice.LoadTesting.Models.Time.Models
{
  public record CompanyUserInfo
  {
    public double? Rate { get; set; }
    public ContractSubjectData ContractSubject { get; set; }
    public DateTime StartWorkingAt { get; set; }
  }
}
