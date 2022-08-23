using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalOffice.LoadTesting.Models.Project.Enums
{
  [JsonConverter(typeof(StringEnumConverter))]
  public enum ProjectUserRoleType
  {
    Manager = 0,
    Employee = 1,
    Observer = 100
  }
}
