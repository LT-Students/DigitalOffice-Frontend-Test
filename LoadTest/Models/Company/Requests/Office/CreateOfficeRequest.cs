namespace DigitalOffice.LoadTesting.Models.Company.Requests.Office
{
    public record CreateOfficeRequest
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}
