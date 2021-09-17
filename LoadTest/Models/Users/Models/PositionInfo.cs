using System;

namespace DigitalOffice.LoadTesting.Models.Users.Models
{
    public record PositionInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime ReceivedAt { get; set; }
    }
}
