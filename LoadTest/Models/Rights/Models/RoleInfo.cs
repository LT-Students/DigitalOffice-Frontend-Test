using LT.DigitalOffice.LoadTesting.Models.Rights.Models;
using System;
using System.Collections.Generic;

namespace DigitalOffice.LoadTesting.Models.Rights.Models
{
  public record RoleInfo
  {
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
    public UserInfo CreatedBy { get; set; }
    public IEnumerable<RightInfo> Rights { get; set; }
    public IEnumerable<RoleLocalizationInfo> Localizations { get; set; }
  }
}
