using System;

namespace DigitalOffice.LoadTesting.Models.Users.Models
{
    public record RoleInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
