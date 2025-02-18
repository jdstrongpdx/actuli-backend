using Newtonsoft.Json;

namespace Actuli.Api.Models;

public class TypeGroup
{
    [JsonProperty("id")] public string Id { get; set; }

    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("description")] public string Description { get; set; }

    [JsonProperty("version")] public int Version { get; set; }

    [JsonProperty("data")] public List<TypeItem> Data { get; set; }

    [JsonProperty("lastUpdated")] public DateTime? LastUpdated { get; set; } = DateTime.Now;
    
    public void MarkAsUpdated()
    {
        LastUpdated = DateTime.UtcNow;
    }
}

public class TypeItem
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("value")] public string Value { get; set; }
}