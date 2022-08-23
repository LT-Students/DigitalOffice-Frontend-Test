using DigitalOffice.LoadTesting.Models.Project.Enums;
using LT.DigitalOffice.LoadTesting.Models.Common;
using System;

namespace LT.DigitalOffice.LoadTesting.Models.Project.Requests.Project.Filters
{
  public record FindProjectsFilter : BaseFindFilter
  {
    public bool? IsAscendingSort { get; set; } = true;
    public ProjectStatusType? ProjectStatus { get; set; }
    public string NameIncludeSubstring { get; set; }
    public bool IncludeDepartment { get; set; } = true;
    public Guid? UserId { get; set; }
    public Guid? DepartmentId { get; set; }
  }
}
