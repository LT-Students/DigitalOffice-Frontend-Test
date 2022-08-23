using LT.DigitalOffice.LoadTesting.Models.Common;
using System;
using System.Collections.Generic;

namespace DigitalOffice.LoadTesting.Models.Time.Filters
{
  public record FindStatFilter : BaseFindFilter
  {
    public List<Guid> DepartmentsIds { get; set; }
    public Guid? ProjectId { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public bool? AscendingSort { get; set; } = true;
    public string NameIncludeSubstring { get; set; }
  }
}
