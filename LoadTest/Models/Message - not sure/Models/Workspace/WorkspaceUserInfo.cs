using System;
using DigitalOffice.LoadTesting.Models.Message.Models.User;

namespace DigitalOffice.LoadTesting.Models.Message.Models.Workspace
{
    public record WorkspaceUserInfo
    {
        public Guid Id { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public UserInfo User { get; set; }
    }
}
