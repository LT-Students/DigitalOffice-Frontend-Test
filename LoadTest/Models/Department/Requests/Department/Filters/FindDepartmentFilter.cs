using LT.DigitalOffice.LoadTesting.Models.Common;

namespace LT.DigitalOffice.LoadTesting.Models.Department.Requests.Department.Filters
{
  public record FindDepartmentFilter : BaseFindFilter
  {
    public bool? IsAscendingSort { get; set; }
    public bool? IsActive { get; set; }
    public string NameIncludeSubstring { get; set; }
  }
}
