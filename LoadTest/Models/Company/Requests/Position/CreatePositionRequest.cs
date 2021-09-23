namespace DigitalOffice.LoadTesting.Models.Company.Requests.Position
{
    public record CreatePositionRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
