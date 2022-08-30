using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace LT.DigitalOffice.LoadTesting.Models.Common.Enums
{
  [JsonConverter(typeof(StringEnumConverter))]
  public enum ContractTerm
  {
    FixedTerm,
    Perpetual
  }
}
