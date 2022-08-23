namespace DigitalOffice.LoadTesting.Models.Company.Requests.Company.Filters
{
  public record GetCompanyFilter
  {
    public bool IncludeOffices { get; set; } = false;
  }
}
