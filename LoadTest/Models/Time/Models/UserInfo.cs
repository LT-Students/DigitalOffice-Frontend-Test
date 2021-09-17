using System;

namespace DigitalOffice.LoadTesting.Models.Time.Models
{
    public record UserInfo
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public float Rate { get; set; }
        public bool IsActive { get; set; }
    }
}
