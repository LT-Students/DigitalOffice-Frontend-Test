namespace LT.DigitalOffice.LoadTesting.Models.Company.Requests.ContractSubject
{
  public record EditContractSubjectRequest
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
  }
}
