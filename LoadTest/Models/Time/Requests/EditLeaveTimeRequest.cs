using DigitalOffice.LoadTesting.Models.Time.Enums;
using System;

namespace DigitalOffice.LoadTesting.Models.Time.Requests
{
  public record EditLeaveTimeRequest
  {
    public int Minutes { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public LeaveType LeaveType { get; set; }
    public string Comment { get; set; }
    public bool IsActive { get; set; }
  }
}
