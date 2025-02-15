using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Actuli.Api.Models;

public class Goal
{
    [JsonProperty("id")]
    [Required]
    public string Id { get; set; }
    
    [JsonProperty("owner")]
    [Required]
    public required string Owner { get; set; } 
    
    [JsonProperty("description")]
    public required string Description { get; set; } = string.Empty;
}