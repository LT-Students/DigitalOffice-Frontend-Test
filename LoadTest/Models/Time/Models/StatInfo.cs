using System.Collections.Generic;

namespace DigitalOffice.LoadTesting.Models.Time.Models
{
    public record StatInfo
    {
        public UserInfo User { get; set; }

        public List<LeaveTimeInfo> LeaveTimes { get; set; }

        public List<WorkTimeInfo> WorkTimes { get; set; }

        public WorkTimeMonthLimitInfo LimitInfo { get; set; }
    }
}
