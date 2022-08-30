using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalOffice.LoadTesting.Models.Users.Enums
{
  [JsonConverter(typeof(StringEnumConverter))]
  public enum CommunicationType
  {
    Email,
    Telegram,
    Phone,
    Skype,
    Twitter,
    BaseEmail
  }
}
