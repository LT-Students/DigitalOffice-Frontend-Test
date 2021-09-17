using System;

namespace DigitalOffice.LoadTesting.Models.Company.Requests.Filters
{
    public record GetDepartmentFilter
    {
        public Guid DepartmentId { get; set; }
        public bool IncludeUsers { get; set; } = true;
        public bool IncludeProjects { get; set; } = true;
    }
}
