using System;
using System.Collections.Generic;
using DigitalOffice.LoadTesting.Models.Message.Models.Image;

namespace DigitalOffice.LoadTesting.Models.Message.Requests.Channel
{
    public record CreateChannelRequest
    {
        public string Name { get; set; }
        public Guid WorkspaceId { get; set; }
        public bool IsPrivate { get; set; }
        public ImageConsist Image { get; set; }
        public List<CreateChannelUserRequest> Users { get; set; }
    }
}
