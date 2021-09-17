using System;

namespace DigitalOffice.LoadTesting.Models.Users.Requests.Credentials.Filters
{
    public record GetCredentialsFilter
    {
        public Guid? UserId { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public override string ToString()
        {
            return $"UserId: {UserId}, Login: {Login}, Email: {Email}, Phone: {Phone}";
        }
    }
}
