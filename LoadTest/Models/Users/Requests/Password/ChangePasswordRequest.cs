namespace DigitalOffice.LoadTesting.Models.Users.Requests.Password
{
  public record ChangePasswordRequest
  {
    public string Password { get; set; }
    public string NewPassword { get; set; }
  }
}
