using DigitalOffice.LoadTesting.Models.Project.Models;
using System;
using System.Collections.Generic;

namespace DigitalOffice.LoadTesting.Models.Project.Requests
{
    public class CreateTaskPropertyRequest
    {
        public Guid ProjectId { get; set; }
        public IEnumerable<TaskProperty> TaskProperties { get; set; }
    }
}
