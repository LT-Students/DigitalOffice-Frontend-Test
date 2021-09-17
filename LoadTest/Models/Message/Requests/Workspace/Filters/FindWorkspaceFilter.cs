namespace DigitalOffice.LoadTesting.Models.Message.Requests.Workspace.Filters
{
    public record FindWorkspaceFilter
    {
        public int SkipCount { get; set; }
        public int TakeCount { get; set; }
        public bool IncludeDeactivated { get; set; } = true;
    }
}
