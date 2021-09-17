using DigitalOffice.LoadTesting.Models.Project.Enums;
using System;

namespace DigitalOffice.LoadTesting.Models.Project.Requests
{
    public class EditProjectRequest
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public ProjectStatusType Status { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
