using DigitalOffice.LoadTesting.Models.Project.Models;
using LT.DigitalOffice.LoadTesting.Models.Project.Models;
using System.Collections.Generic;

namespace LT.DigitalOffice.ProjectService.Models.Dto.Responses
{
  public class ProjectResponse
  {
    public ProjectInfo Project { get; set; }
    public IEnumerable<ProjectUserInfo> Users { get; set; }
  }
}
