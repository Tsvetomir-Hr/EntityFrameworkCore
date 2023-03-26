using Footballers.Common;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Footballers.DataProcessor.ImportDto;

[JsonObject]
public class ImportTeamWithFootballersDto
{
    [Required]
    [JsonProperty("Name")]
    [MinLength(ValidationConstants.TeamNameMinLength)]
    [MaxLength(ValidationConstants.TeamNameMaxLength)]
    [RegularExpression("[A-za-z0-9\\s\\.\\-]*")]
    public string Name { get; set; } = null!;

    [Required]
    [JsonProperty("Nationality")]
    [MinLength(ValidationConstants.NationalityMinLength)]
    [MaxLength(ValidationConstants.NationalityMaxLength)]
    public string Nationality { get; set; } = null!;

    [Required]
    [JsonProperty("Trophies")]
    public string Trophies { get; set; } = null!;

    [JsonProperty("Footballers")]
    [Required]
    public HashSet<int> FootballersIds { get; set; } = null!;
}
