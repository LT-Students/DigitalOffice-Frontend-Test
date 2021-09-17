using DigitalOffice.LoadTesting.Models.Company.Models;
using System.Collections.Generic;

namespace DigitalOffice.LoadTesting.Models.Company.Responses
{
    public record DepartmentResponse
    {
        public DepartmentInfo Department { get; set; }
        public IEnumerable<UserInfo> Users { get; set; }
        public IEnumerable<ProjectInfo> Projects { get; set; }
    }
}
