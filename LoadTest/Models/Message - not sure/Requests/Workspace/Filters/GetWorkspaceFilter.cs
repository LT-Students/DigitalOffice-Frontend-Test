using System;

namespace DigitalOffice.LoadTesting.Models.Message.Requests.Workspace.Filters
{
    public record GetWorkspaceFilter
    {
        public Guid WorkspaceId { get; set; }
        public bool IncludeUsers { get; set; } = true;
        public bool IncludeChannels { get; set; } = true;
    }
}
