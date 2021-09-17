using System;
using System.Collections.Generic;

namespace DigitalOffice.LoadTesting.Models.Company.Requests.Department
{
    public record CreateDepartmentRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? DirectorUserId { get; set; }
        public IEnumerable<Guid> Users { get; set; }
    }
}
