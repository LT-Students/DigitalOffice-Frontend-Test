using System;

namespace LT.DigitalOffice.LoadTesting.Models.Department.Requests.Department.Filters
{
  public record GetDepartmentFilter
  {
    public Guid DepartmentId { get; set; }
    public bool IncludeUsers { get; set; } = true;
    public bool IncludeCategory { get; set; } = true;
  }
}
