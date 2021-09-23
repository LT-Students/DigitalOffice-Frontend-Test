using System.Collections.Generic;

namespace DigitalOffice.LoadTesting.Models.Project.Responses
{
    public class FindResponse<T>
    {
        public int TotalCount { get; set; }
        public IEnumerable<T> Body { get; set; } = new List<T>();
        public IEnumerable<string> Errors { get; set; } = new List<string>();
    }
}