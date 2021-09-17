namespace DigitalOffice.LoadTesting.Models.Company.Requests.Position
{
    public record EditPositionRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
