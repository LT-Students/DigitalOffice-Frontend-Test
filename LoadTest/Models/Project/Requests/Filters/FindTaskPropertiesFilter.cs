using System;

namespace DigitalOffice.LoadTesting.Models.Project.Requests.Filters
{
    public class FindTaskPropertiesFilter
    {
        public string Name { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? AuthorId { get; set; }
    }
}
