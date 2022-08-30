using DigitalOffice.LoadTesting.Models.Project.Enums;
using System;

namespace LT.DigitalOffice.LoadTesting.Models.Project.Requests.Project
{
  public record EditProjectRequest
  {
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Description { get; set; }
    public string ShortDescription { get; set; }
    public string Customer { get; set; }
    public DateTime? StartDateUtc { get; set; }
    public DateTime? EndDateUtc { get; set; }
    public ProjectStatusType Status { get; set; }
  }
}
