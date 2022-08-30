using System;

namespace LT.DigitalOffice.LoadTesting.Models.Project.Requests.Department
{
  public class EditProjectDepartmentRequest
  {
    public Guid ProjectId { get; set; }
    public Guid? DepartmentId { get; set; }
  }
}
