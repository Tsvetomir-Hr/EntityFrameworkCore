namespace TeisterMask.DataProcessor.ImportDto;


using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations;

[JsonObject]
public class ImportEmployeeDto
{
    [JsonProperty("Username")]
    [Required]
    [MinLength(GlobalConstants.UsernameMinLength)]
    [MaxLength(GlobalConstants.UsernameMaxLength)]
    [RegularExpression(@"[a-zA-Z0-9]+")]
    public string Username { get; set; } = null!;

    [JsonProperty("Email")]
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [JsonProperty("Phone")]
    [Required]
    [RegularExpression(@"^[0-9]{3}-[0-9]{3}-[0-9]{4}$")]
    public string Phone { get; set; } = null!;

    [JsonProperty("Tasks")]
    public HashSet<int> Tasks { get; set; } = null!;
}
