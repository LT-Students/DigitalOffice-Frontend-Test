using System;

namespace LT.DigitalOffice.LoadTesting.Models.Project.Requests.Project.Filters
{
  public class GetProjectFilter
  {
    public Guid ProjectId { get; set; }
    public bool IncludeDepartment { get; set; } = true;
    public bool IncludeProjectUsers { get; set; } = true;
  }
}
