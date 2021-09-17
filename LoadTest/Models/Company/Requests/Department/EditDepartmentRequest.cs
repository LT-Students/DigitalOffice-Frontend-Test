using System;

namespace DigitalOffice.LoadTesting.Models.Company.Requests.Department
{
    public record EditDepartmentRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid DirectorId { get; set; }
        public bool IsActive { get; set; }
    }
}
