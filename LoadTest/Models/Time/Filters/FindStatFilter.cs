using System;

namespace DigitalOffice.LoadTesting.Models.Time.Filters
{
    public record FindStatFilter
    {
        public Guid? DepartmentId { get; set; }
        public Guid? ProjectId { get; set; }
        public int SkipCount { get; set; }
        public int TakeCount { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
