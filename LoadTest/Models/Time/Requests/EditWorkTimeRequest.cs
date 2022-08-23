namespace DigitalOffice.LoadTesting.Models.Time.Requests
{
  public record EditWorkTimeRequest
  {
    public float? Hours { get; set; }
    public string Description { get; set; }
  }
}
