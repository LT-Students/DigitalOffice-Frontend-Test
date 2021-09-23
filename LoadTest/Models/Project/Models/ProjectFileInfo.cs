using System;

namespace DigitalOffice.LoadTesting.Models.Project.Models
{
    public record ProjectFileInfo
    {
        public Guid ProjectId { get; set; }
        public Guid FileId { get; set; }
    }
}
