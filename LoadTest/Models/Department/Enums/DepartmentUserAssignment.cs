using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace LT.DigitalOffice.LoadTesting.Models.Department.Enums
{
  [JsonConverter(typeof(StringEnumConverter))]
  public enum DepartmentUserAssignment
  {
    Employee = 0,
    Director = 10
  }
}
