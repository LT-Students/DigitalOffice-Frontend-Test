using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalOffice.LoadTesting.Models.Message.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatusType
    {
        Sent = 0,
        Read = 1,
        Removed = 2
    }
}
