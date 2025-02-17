using Newtonsoft.Json;

namespace Actuli.Api.Types;

public class TypeData
{
    [JsonProperty("id")] public required string Id { get; set;  } 

    [JsonProperty("types")] public List<TypeItem> Types { get; set; }
}