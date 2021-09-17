using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace DigitalOffice.LoadTesting.Models.Project.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ImageType
    {
        Project,
        Task
    }
}
