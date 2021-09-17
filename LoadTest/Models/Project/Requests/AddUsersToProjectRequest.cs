using DigitalOffice.LoadTesting.Models.Project.Models.ProjectUser;
using System;
using System.Collections.Generic;

namespace DigitalOffice.LoadTesting.Models.Project.Requests
{
    public class AddUsersToProjectRequest
    {
        public Guid ProjectId { get; set; }
        public IEnumerable<ProjectUserRequest> Users { get; set; }
    }
}