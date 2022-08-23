using DigitalOffice.LoadTesting.Models.Company.Models;

namespace DigitalOffice.LoadTesting.Models.Company.Requests.Company
{
  public record CreateCompanyRequest
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public string Tagline { get; set; }
    public string Contacts { get; set; }
    public ImageConsist Logo { get; set; }
  }
}
