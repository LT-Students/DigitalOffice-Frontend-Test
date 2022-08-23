using LT.DigitalOffice.LoadTesting.Models.Department.Requests.User;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.LoadTesting.Models.Department.Requests.Department
{
  public record CreateDepartmentRequest
  {
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Description { get; set; }
    public Guid? ParentId { get; set; }
    public Guid? CategoryId { get; set; }
    public List<CreateUserRequest> Users { get; set; }
  }
}
