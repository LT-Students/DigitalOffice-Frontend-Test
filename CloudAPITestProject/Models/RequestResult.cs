using System.Collections.Generic;

namespace CloudAPITestProject.Models
{
    public class RequestResult
    {
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
