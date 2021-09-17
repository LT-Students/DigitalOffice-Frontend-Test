namespace DigitalOffice.LoadTesting.Models.Company.Requests.Company
{
    public record AddImageRequest
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string Extension { get; set; }
    }
}
