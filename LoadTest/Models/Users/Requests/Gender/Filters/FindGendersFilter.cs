using LT.DigitalOffice.LoadTesting.Models.Common;

namespace LT.DigitalOffice.LoadTesting.Models.Users.Requests.Gender.Filters
{
  public record FindGendersFilter : BaseFindFilter
  {
    public string NameIncludeSubstring { get; set; }
  }
}
