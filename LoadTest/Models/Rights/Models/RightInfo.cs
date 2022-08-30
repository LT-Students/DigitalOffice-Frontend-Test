namespace LT.DigitalOffice.LoadTesting.Models.Rights.Models
{
  public record RightInfo
  {
    public int RightId { get; set; }
    public string Locale { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
  }
}
