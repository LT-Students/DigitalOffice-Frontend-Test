namespace DigitalOffice.LoadTesting.Models.Auth.Requests
{
  public record LoginRequest
  {
    public string LoginData { get; set; }
    public string Password { get; set; }
  }
}