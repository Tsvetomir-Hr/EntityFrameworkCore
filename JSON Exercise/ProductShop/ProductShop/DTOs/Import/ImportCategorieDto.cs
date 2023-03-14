using Newtonsoft.Json;

namespace ProductShop.DTOs.Import;

[JsonObject]
public class ImportCategorieDto
{
    [JsonProperty("name")]
    public string? Name { get; set; }
}
