using LT.DigitalOffice.LoadTesting.Models.Common;
using LT.DigitalOffice.LoadTesting.Models.Common.Enums;
using System;

namespace LT.DigitalOffice.LoadTesting.Models.Users.Models
{
  public record CompanyUserInfo
  {
    public CompanyInfo Company { get; set; }
    public ContractSubjectData ContractSubject { get; set; }
    public ContractTerm ContractTermType { get; set; }
    public double? Rate { get; set; }
    public DateTime StartWorkingAt { get; set; }
    public DateTime? EndWorkingAt { get; set; }
    public DateTime? Probation { get; set; }
  }
}
