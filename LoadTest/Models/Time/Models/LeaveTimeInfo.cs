using DigitalOffice.LoadTesting.Models.Time.Enums;
using System;

namespace DigitalOffice.LoadTesting.Models.Time.Models
{
  public record LeaveTimeInfo
  {
    public Guid Id { get; set; }
    public Guid CreatedBy { get; set; }
    public int Minutes { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public LeaveType LeaveType { get; set; }
    public string Comment { get; set; }
    public bool IsActive { get; set; }
  }
}
