using System.Collections.Generic;
using DigitalOffice.LoadTesting.Models.Message.Models.Image;

namespace DigitalOffice.LoadTesting.Models.Message.Requests.Workspace
{
    public record CreateWorkspaceRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ImageConsist Image { get; set; }
        public List<WorkspaceUserRequest> Users { get; set; }
    }
}
