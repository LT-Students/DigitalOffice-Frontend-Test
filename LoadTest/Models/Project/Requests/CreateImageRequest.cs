using DigitalOffice.LoadTesting.Models.Project.Enums;
using DigitalOffice.LoadTesting.Models.Project.Models;
using System;
using System.Collections.Generic;

namespace DigitalOffice.LoadTesting.Models.Project.Requests
{
    public record CreateImageRequest
    {
        public Guid ProjectOrTaskId { get; set; }
        public List<ImageContent> Images { get; set; }
        public ImageType ImageType { get; set; }
    }
}
