using DigitalOffice.LoadTesting.Models.Time.Models;

namespace DigitalOffice.LoadTesting.Models.Time.Responses
{
  public record WorkTimeResponse
  {
    public WorkTimeInfo WorkTime { get; set; }
    public UserInfo User { get; set; }
    public WorkTimeMonthLimitInfo LimitInfo { get; set; }
  }
}
