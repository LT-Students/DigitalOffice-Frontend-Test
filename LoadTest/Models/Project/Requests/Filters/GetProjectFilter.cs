using System;

namespace DigitalOffice.LoadTesting.Models.Project.Requests.Filters
{
    public class GetProjectFilter
    {
        public Guid ProjectId { get; set; }
        public bool IncludeUsers { get; set; } = true;
        public bool ShowNotActiveUsers { get; set; } = true;
        public bool IncludeFiles { get; set; } = true;
        public bool IncludeImages { get; set; } = true;
    }
}
