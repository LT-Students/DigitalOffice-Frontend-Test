using DigitalOffice.LoadTesting.Models.Project.Enums;
using System;

namespace DigitalOffice.LoadTesting.Models.Project.Requests
{
    public record RemoveImageRequest
    {
        public Guid ImageId { get; set; }
        public ImageType ImageType { get; set; }
    }
}
