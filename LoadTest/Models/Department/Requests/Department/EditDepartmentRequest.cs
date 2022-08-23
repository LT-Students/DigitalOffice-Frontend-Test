using System;

namespace LT.DigitalOffice.LoadTesting.Models.Department.Requests.Department
{
  public record EditDepartmentRequest
  {
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public Guid? CategoryId { get; set; }
  }
}
