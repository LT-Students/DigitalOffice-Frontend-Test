using DigitalOffice.LoadTesting.Models.Time.Models;

namespace DigitalOffice.LoadTesting.Models.Time.Responses
{
  public record LeaveTimeResponse
  {
    public LeaveTimeInfo LeaveTime { get; set; }
    public UserInfo User { get; set; }
  }
}
