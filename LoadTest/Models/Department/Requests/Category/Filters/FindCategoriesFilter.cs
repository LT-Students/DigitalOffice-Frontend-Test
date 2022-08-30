using LT.DigitalOffice.LoadTesting.Models.Common;

namespace LT.DigitalOffice.LoadTesting.Models.Department.Requests.Category.Filters
{
  public record FindCategoriesFilter : BaseFindFilter
  {
    public string NameIncludeSubstring { get; set; }
    public bool? IsAscendingSort { get; set; }
  }
}
