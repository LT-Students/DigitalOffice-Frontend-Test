using LT.DigitalOffice.LoadTesting.Models.Common;

namespace DigitalOffice.LoadTesting.Models.Time.Filters
{
  public record FindWorkTimeMonthLimitsFilter : BaseFindFilter
  {
    public int? Year { get; set; }
    public int? Month { get; set; }
  }
}
