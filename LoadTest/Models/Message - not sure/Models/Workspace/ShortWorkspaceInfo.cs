using System;
using DigitalOffice.LoadTesting.Models.Message.Models.Image;

namespace DigitalOffice.LoadTesting.Models.Message.Models.Workspace
{
    public record ShortWorkspaceInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public ImageConsist Image { get; set; }
    }
}
