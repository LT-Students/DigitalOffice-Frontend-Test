using System;

namespace DigitalOffice.LoadTesting.Models.Project.Requests.Filters
{
    public class FindDbProjectsUserFilter
    {
        public Guid? UserId { get; set; }
        public bool IncludeProject { get; set; } = true;
    }
}
