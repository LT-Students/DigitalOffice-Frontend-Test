using System;
using DigitalOffice.LoadTesting.Models.Message.Models.Image;

namespace DigitalOffice.LoadTesting.Models.Message.Models.User
{
    public record UserInfo
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public ImageInfo Avatar { get; set; }
    }
}
