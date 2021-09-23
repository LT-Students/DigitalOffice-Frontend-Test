using System;

namespace DigitalOffice.LoadTesting.Models.Project.Models
{
    public record ProjectTaskInfo
    {
        public Guid Id { get; set; }
        public string ShortName { get; set; }
    }
}
