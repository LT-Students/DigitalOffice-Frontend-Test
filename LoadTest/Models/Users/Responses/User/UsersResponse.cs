using DigitalOffice.LoadTesting.Models.Users.Models;
using System.Collections.Generic;

namespace DigitalOffice.LoadTesting.Models.Users.Responses.User
{
    public record UsersResponse
    {
        public int TotalCount { get; set; }
        public List<UserInfo> Users { get; set; } = new();
        public List<string> Errors { get; set; } = new();
    }
}
