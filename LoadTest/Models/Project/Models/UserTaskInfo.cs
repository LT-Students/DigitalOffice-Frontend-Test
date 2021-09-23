using System;

namespace DigitalOffice.LoadTesting.Models.Project.Models
{
    public record UserTaskInfo
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
}
