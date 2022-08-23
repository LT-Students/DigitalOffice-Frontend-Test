using DigitalOffice.LoadTesting.Models.Users.Enums;

namespace LT.DigitalOffice.LoadTesting.Models.Users.Requests.Communication
{
  public record EditCommunicationRequest
  {
    public CommunicationType? Type { get; set; }
    public string Value { get; set; }
  }
}
