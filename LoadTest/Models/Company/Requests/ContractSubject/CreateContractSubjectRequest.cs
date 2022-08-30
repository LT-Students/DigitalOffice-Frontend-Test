namespace LT.DigitalOffice.LoadTesting.Models.Company.Requests.ContractSubject
{
  public record CreateContractSubjectRequest
  {
    public string Name { get; set; }
    public string Description { get; set; }
  }
}
