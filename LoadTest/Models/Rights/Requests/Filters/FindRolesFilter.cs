using LT.DigitalOffice.LoadTesting.Models.Common;

namespace LT.DigitalOffice.LoadTesting.Models.Rights.Requests.Filters
{
  public record FindRolesFilter : BaseFindFilter
  {
    public bool IncludeDeactivated { get; set; } = false;
    public string Locale { get; set; }
  }
}
