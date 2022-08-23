using LT.DigitalOffice.LoadTesting.Models.Rights.Requests;
using System.Collections.Generic;

namespace DigitalOffice.LoadTesting.Models.Rights.Requests
{
  public record CreateRoleRequest
  {
    public List<CreateRoleLocalizationRequest> Localizations { get; set; }
    public List<int> Rights { get; set; }
  }
}
