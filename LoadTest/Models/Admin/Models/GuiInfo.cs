using System;

namespace LT.DigitalOffice.LoadTesting.Models.Admin.Models
{
  public class GuiInfo
  {
    public Guid? Id { get; set; }
    public string PortalName { get; set; }
    public string SiteUrl { get; set; }
    public ImageConsist Logo { get; set; }
    public ImageConsist Favicon { get; set; }
  }
}
