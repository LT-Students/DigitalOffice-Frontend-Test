using System;

namespace LT.DigitalOffice.LoadTesting.Models.Users.Requests.Credentials
{
  public record ReactivateCredentialsRequest
  {
    public Guid UserId { get; set; }
    public string Password { get; set; }
  }
}
