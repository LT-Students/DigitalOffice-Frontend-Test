using System;

namespace DigitalOffice.LoadTesting.Models.Users.Requests.Password
{
    public class ChangePasswordRequest
    {
        public Guid UserId { get; set; }
        public Guid Secret { get; set; }
        public string Login { get; set; }
        public string NewPassword { get; set; }
    }
}