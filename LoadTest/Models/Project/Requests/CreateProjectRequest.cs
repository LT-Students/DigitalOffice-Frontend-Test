using DigitalOffice.LoadTesting.Models.Project.Enums;
using DigitalOffice.LoadTesting.Models.Project.Models;
using DigitalOffice.LoadTesting.Models.Project.Models.ProjectUser;
using System;
using System.Collections.Generic;

namespace DigitalOffice.LoadTesting.Models.Project.Requests
{
    public class CreateProjectRequest
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public Guid? DepartmentId { get; set; }
        public ProjectStatusType Status { get; set; }
        public IEnumerable<ProjectUserRequest> Users { get; set; }
        public IEnumerable<ImageContent> ProjectImages { get; set; }
    }
}
