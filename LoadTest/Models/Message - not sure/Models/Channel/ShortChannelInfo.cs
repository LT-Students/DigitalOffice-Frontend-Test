using System;
using DigitalOffice.LoadTesting.Models.Message.Models.Image;

namespace DigitalOffice.LoadTesting.Models.Message.Models.Channel
{
    public record ShortChannelInfo
    {
        public Guid Id { get; set; }
        public ImageConsist Avatar { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsPrivate { get; set; }
    }
}
