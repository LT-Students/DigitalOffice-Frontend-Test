using System;

namespace DigitalOffice.LoadTesting.Models.Users.Models
{
    public record ProjectInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }
}
