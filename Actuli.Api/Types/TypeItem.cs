using Newtonsoft.Json;

namespace Actuli.Api.Types;

public class TypeItem
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("description")] public string Description { get; set; }

    [JsonProperty("version")] public int Version { get; set; }

    [JsonProperty("data")] public List<TypeListItem> Data { get; set; }

    [JsonProperty("lastUpdated")] public DateTime? LastUpdated { get; set; }
}