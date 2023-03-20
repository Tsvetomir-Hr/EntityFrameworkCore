using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Theatre.Common;

namespace Theatre.DataProcessor.ImportDto;

[JsonObject]
public class ImportTheatersTicketsDto
{
    [Required]
    [JsonProperty("Name")]
    [MinLength(ValidationConstants.TheatreNameMinLength)]
    [MaxLength(ValidationConstants.TheatreNameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    [JsonProperty("NumberOfHalls")]
    [Range(ValidationConstants.TheatreHallsMinNumber, ValidationConstants.TheatreHallsMaxNumber)]
    public sbyte NumberOfHalls { get; set; }

    [Required]
    [JsonProperty("Director")]
    [MinLength(ValidationConstants.TheatreDirectorMinLength)]
    [MaxLength(ValidationConstants.TheatreDirectorMaxLength)]
    public string Director { get; set; } = null!;

    [JsonProperty("Tickets")]
    public ImportTicketDto[] Tickets { get; set; } = null!;
}
