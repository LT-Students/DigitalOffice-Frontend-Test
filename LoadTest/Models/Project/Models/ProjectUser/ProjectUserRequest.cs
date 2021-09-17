using DigitalOffice.LoadTesting.Models.Project.Enums;
using System;

namespace DigitalOffice.LoadTesting.Models.Project.Models.ProjectUser
{
    public class ProjectUserRequest
    {
        public Guid UserId { get; set; }
        public ProjectUserRoleType Role { get; set; }
    }
}
