using System;

namespace DigitalOffice.LoadTesting.Models
{
  public record ScenarioStartSettings
  {
    public string URL { get; set; }
    public string Path { get; set; }
    public int Rate { get; set; }
    public TimeSpan During { get; set; }
    public TimeSpan WarmUpTime { get; set; }
    public TimeSpan ResponseTimeout { get; set; }
    public string Token { get; set; }
  }
}
