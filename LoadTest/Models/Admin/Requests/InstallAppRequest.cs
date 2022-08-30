using LT.DigitalOffice.LoadTesting.Models.Admin.Models;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.LoadTesting.Models.Admin.Requests
{
  public record InstallAppRequest
  {
    public SmtpInfo SmtpInfo { get; set; }
    public AdminInfo AdminInfo { get; set; }
    public GuiInfo GuiInfo { get; set; }
    public List<Guid> ServicesToDisable { get; set; }
  }
}
