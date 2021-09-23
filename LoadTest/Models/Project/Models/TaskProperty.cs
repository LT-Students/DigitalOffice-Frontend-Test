using DigitalOffice.LoadTesting.Models.Project.Enums;
using System;

namespace DigitalOffice.LoadTesting.Models.Project.Models
{
    public record TaskProperty
    {
        public string Name { get; set; }
        public TaskPropertyType PropertyType { get; set; }
        public string Description { get; set; }
    }
}
