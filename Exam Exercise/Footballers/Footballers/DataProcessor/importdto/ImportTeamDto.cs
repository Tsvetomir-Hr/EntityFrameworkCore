using Footballers.Common;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Footballers.DataProcessor.ImportDto;

[JsonObject]
public class ImportTeamDto
{
    [Required]
    [MinLength(GlobalConstants.TeamNameMinLength)]
    [MaxLength(GlobalConstants.TeamNameMaxLength)]
    [RegularExpression("^[A-z0-9\\s\\.\\-]+$")]
    [JsonProperty("Name")]
    public string Name { get; set; } = null!;

    [Required]
    [MinLength(GlobalConstants.TeamNationalityMinLength)]
    [MaxLength(GlobalConstants.TeamNationalityMaxLength)]
    [JsonProperty("Nationality")]
    public string Nationality { get; set; } = null!;

    [JsonProperty("Trophies")]
    public int Trophies { get; set; }

    [JsonProperty("Footballers")]
    public HashSet<int> Footballers { get; set; }
}
