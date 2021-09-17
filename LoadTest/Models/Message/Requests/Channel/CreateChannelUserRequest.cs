using System;

namespace DigitalOffice.LoadTesting.Models.Message.Requests.Channel
{
    public record CreateChannelUserRequest
    {
        public Guid WorkspaceUserId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
