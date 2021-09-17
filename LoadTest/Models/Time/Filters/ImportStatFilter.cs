using System;

namespace DigitalOffice.LoadTesting.Models.Time.Filters
{
    public record ImportStatFilter
    {
        public Guid? DepartmentId { get; set; }
        public Guid? ProjectId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
