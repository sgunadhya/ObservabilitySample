using System.Text.Json.Serialization;

namespace BoredClassLib;

public class Bored
{
    [JsonPropertyName("activity")]
    public string Activity { get; set; }
    
    [JsonPropertyName("type")]
    public string Type { get; set; }
    
    [JsonPropertyName("participants")]
    public int Participants { get; set; }
    
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
    
    [JsonPropertyName("key")]
    public string Key { get; set; }
    
    [JsonPropertyName("accessibility")]
    public decimal Accessibility { get; set; }
}