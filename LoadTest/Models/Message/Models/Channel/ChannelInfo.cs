using System;
using System.Collections.Generic;
using DigitalOffice.LoadTesting.Models.Message.Models.Image;
using DigitalOffice.LoadTesting.Models.Message.Models.User;
using DigitalOffice.LoadTesting.Models.Message.Models.Workspace;

namespace DigitalOffice.LoadTesting.Models.Message.Models.Channel
{
    public record ChannelInfo
    {
        public Guid Id { get; set; }
        public ShortWorkspaceInfo WorkspaceId { get; set; }
        public ImageConsist Avatar { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsPrivate { get; set; }

        public List<UserInfo> Users { get; set; }
    }
}
