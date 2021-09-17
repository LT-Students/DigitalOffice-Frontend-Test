using System;

namespace DigitalOffice.LoadTesting.Models.Project.Requests.Filters
{
    public class FindTasksFilter
    {
        public int? Number { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? AssignedTo { get; set; }
        public Guid? Status { get; set; }
    }
}
