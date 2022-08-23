using LT.DigitalOffice.LoadTesting.Models.Common;
using System;

namespace DigitalOffice.LoadTesting.Models.Time.Filters
{
  public record FindWorkTimesFilter : BaseFindFilter
  {
    public Guid? UserId { get; set; }
    public Guid? ProjectId { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }
    public bool? IncludeDeactivated { get; set; } = true;
    public bool? IncludeDayJobs { get; set; } = false;
  }
}
