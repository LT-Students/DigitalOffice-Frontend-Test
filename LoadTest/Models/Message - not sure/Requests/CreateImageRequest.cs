namespace DigitalOffice.LoadTesting.Models.Message.Requests
{
    public record CreateImageRequest
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string Extension { get; set; }
    }
}
