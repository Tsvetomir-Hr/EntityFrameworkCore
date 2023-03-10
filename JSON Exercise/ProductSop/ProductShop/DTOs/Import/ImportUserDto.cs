using ProductShop.Common;

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ProductShop.DTOs.Import;

[JsonObject]

public class ImportUserDto
{
    [JsonProperty("firstName")]
    public string FirstName { get; set; }

    [JsonProperty("lastName")]
    [Required]
    [MinLength(GlobalConstants.UserLastNameMinLength)]
    public string LastName { get; set; }
    
    [JsonProperty("age")]
    public int? Age { get; set; }
}
