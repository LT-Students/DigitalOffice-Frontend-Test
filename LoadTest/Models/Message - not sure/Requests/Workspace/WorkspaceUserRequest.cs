using System;

namespace DigitalOffice.LoadTesting.Models.Message.Requests.Workspace
{
    public record WorkspaceUserRequest
    {
        public Guid UserId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
