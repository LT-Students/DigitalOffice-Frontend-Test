using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalOffice.LoadTesting.Models.Message.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ServiceName
    {
        UserService,
        ProjectService
    }
}
