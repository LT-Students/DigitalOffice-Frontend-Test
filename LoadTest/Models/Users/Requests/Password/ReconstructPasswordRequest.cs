using System;

namespace LT.DigitalOffice.LoadTesting.Models.Users.Requests.Password
{
  public record ReconstructPasswordRequest
  {
    public Guid UserId { get; set; }
    public string Secret { get; set; }
    public string Login { get; set; }
    public string NewPassword { get; set; }
  }
}
