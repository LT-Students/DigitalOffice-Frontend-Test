using LT.DigitalOffice.LoadTesting.Models.Project.Models;
using System;

namespace DigitalOffice.LoadTesting.Models.Users.Models
{
  public record ProjectInfo
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Status { get; set; }
    public string ShortDescription { get; set; }
    public ProjectUserInfo User { get; set; }
  }
}
