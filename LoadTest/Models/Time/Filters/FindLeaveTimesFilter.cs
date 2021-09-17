using System;

namespace DigitalOffice.LoadTesting.Models.Time.Filters
{
    public class FindLeaveTimesFilter
    {
        public Guid? UserId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int SkipCount { get; set; }
        public int TakeCount { get; set; }
        public bool IncludeDeactivated { get; set; } = true;
    }
}
