using System;
using System.Collections.Generic;
using DigitalOffice.LoadTesting.Models.Message.Models.Channel;
using DigitalOffice.LoadTesting.Models.Message.Models.Image;
using DigitalOffice.LoadTesting.Models.Message.Models.User;

namespace DigitalOffice.LoadTesting.Models.Message.Models.Workspace
{
    public record WorkspaceInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public ImageConsist Image { get; set; }
        public UserInfo CreatedBy { get; set; }
        public DateTime CreatedAtUtc { get; set; }

        public List<ShortChannelInfo> Channels { get; set; }
        public List<UserInfo> Users { get; set; }
    }
}
