using System;

namespace DigitalOffice.LoadTesting.Models.Time.Filters
{
    public class FindWorkTimesFilter
    {
        public Guid? UserId { get; set; }
        public Guid? ProjectId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public int SkipCount { get; set; }
        public int TakeCount { get; set; }
        public bool IncludeDeactivated { get; set; } = true;
        public bool IncludeDayJobs { get; set; } = true;
    }
}
