using LT.DigitalOffice.LoadTesting.Models.Common;
using System;

namespace DigitalOffice.LoadTesting.Models.Time.Filters
{
  public record FindLeaveTimesFilter : BaseFindFilter
  {
    public Guid? UserId { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public bool? IncludeDeactivated { get; set; } = true;
  }
}
