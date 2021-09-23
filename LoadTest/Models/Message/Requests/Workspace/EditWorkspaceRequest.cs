using DigitalOffice.LoadTesting.Models.Message.Models.Image;

namespace DigitalOffice.LoadTesting.Models.Message.Requests.Workspace
{
    public record EditWorkspaceRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public ImageConsist Image { get; set; }
    }
}
