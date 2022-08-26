using LT.DigitalOffice.LoadTesting.Models.Admin.Models;

namespace LT.DigitalOffice.LoadTesting.Models.Admin.Requests
{
  public record EditGraphicalUserInterfaceSettingRequest
  {
    public string PortalName { get; set; }
    public string SiteUrl { get; set; }
    public ImageConsist Logo { get; set; }
    public ImageConsist Favicon { get; set; }
  }
}
