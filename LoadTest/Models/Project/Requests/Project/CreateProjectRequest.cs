using DigitalOffice.LoadTesting.Models.Project.Enums;
using DigitalOffice.LoadTesting.Models.Project.Models;
using LT.DigitalOffice.LoadTesting.Models.Project.Requests.User;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.LoadTesting.Models.Project.Requests.Project
{
  public record CreateProjectRequest
  {
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Description { get; set; }
    public string ShortDescription { get; set; }
    public string Customer { get; set; }
    public DateTime StartDateUtc { get; set; }
    public DateTime? EndDateUtc { get; set; }
    public Guid? DepartmentId { get; set; }
    public ProjectStatusType Status { get; set; }
    public List<ImageContent> ProjectImages { get; set; }
    public List<UserRequest> Users { get; set; }
  }
}
