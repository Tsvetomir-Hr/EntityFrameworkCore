using Newtonsoft.Json;

namespace ProductShop.DTOs.Import;

[JsonObject]
public class InportUserDto
{
    [JsonProperty("firstName")]
    public string? FirstName { get; set; }

    [JsonProperty("lastName")]
    public string LastName { get; set; }

    [JsonProperty("age")]
    public int? Age { get; set; }
}
