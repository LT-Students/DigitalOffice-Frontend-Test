using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalOffice.LoadTesting.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OperationResultStatusType
    {
        FullSuccess,
        PartialSuccess,
        Failed
    }
}
