using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalOffice.LoadTesting.Models.Time.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LeaveType
    {
        Vacation = 0,
        SickLeave = 1,
        Training = 2,
        Idle = 3
    }
}
