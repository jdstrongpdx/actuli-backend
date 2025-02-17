using Newtonsoft.Json;

namespace Actuli.Api.Types;

public class TypeListItem
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("value")] public string Value { get; set; }
}