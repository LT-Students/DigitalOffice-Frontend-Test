using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace DigitalOffice.LoadTesting.Models.Company.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DepartmentUserRole
    {
        Employee,
        Director
    }
}
