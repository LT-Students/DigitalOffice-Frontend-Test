using System.Collections.Generic;

namespace DigitalOffice.LoadTesting.Models.Rights.Requests
{
    public record CreateRoleRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> Rights { get; set; }
    }
}