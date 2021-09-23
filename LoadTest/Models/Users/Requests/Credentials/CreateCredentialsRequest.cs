using System;

namespace DigitalOffice.LoadTesting.Models.Users.Requests.Credentials
{
    public record CreateCredentialsRequest
    {
        public Guid UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
