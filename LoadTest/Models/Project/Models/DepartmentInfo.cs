using System;

namespace DigitalOffice.LoadTesting.Models.Project.Models
{
    public record DepartmentInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
