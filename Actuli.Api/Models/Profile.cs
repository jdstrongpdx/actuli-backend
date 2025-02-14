using Newtonsoft.Json;

namespace Actuli.Api.Models;

public class Profile
{
    [JsonProperty("contact")]
    public Contact? Contact { get; set; }
}