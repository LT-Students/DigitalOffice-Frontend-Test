using System;

namespace DigitalOffice.LoadTesting.Models.Project.Requests
{
    public class EditTaskRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? AssignedTo { get; set; }
        public Guid PriorityId { get; set; }
        public Guid StatusId { get; set; }
        public Guid TypeId { get; set; }
        public int? PlannedMinutes { get; set; }
    }
}
