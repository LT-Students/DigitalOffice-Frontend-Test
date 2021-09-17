using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalOffice.LoadTesting.Models.Users.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FormEducation
    {
        FullTime,
        Distance
    }
}
