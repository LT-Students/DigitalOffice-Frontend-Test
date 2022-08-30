using LT.DigitalOffice.LoadTesting.Models.Common;

namespace LT.DigitalOffice.LoadTesting.Models.Company.Requests.ContractSubject.Filters
{
  public record FindContractSubjectFilter : BaseFindFilter
  {
    public bool? IsActive { get; set; }
  }
}
