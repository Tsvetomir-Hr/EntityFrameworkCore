using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProductShop.DTOs.Import;

[JsonObject]
public class ImportCategoryDto
{
    [JsonProperty("Name")]
    [Required]
    public string Name { get; set; }
}
