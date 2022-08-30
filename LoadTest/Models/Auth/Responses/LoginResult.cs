using System;

namespace DigitalOffice.LoadTesting.Models.Auth.Responses
{
  public record LoginResult
  {
    public Guid UserId { get; init; }
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
    public double AccessTokenExpiresIn { get; init; }
    public double RefreshTokenExpiresIn { get; init; }
  }
}