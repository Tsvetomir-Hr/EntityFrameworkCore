using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using VaporStore.Common;

namespace VaporStore.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportUserDto
    {
        [Required]
        [JsonProperty("FullName")]
        [RegularExpression("^[A-Z][a-z]+\\s[A-Z][a-z]+$")]
        public string FullName { get; set; } = null!;

        [Required]
        [MinLength(GlobalConstants.UsernameMinLength)]
        [MaxLength(GlobalConstants.UsernameMaxLength)]
        [JsonProperty("Username")]
        public string Username { get; set; } = null!;

        [Required]
        [JsonProperty("Email")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [JsonProperty("Age")]
        [Range(GlobalConstants.UserAgeMin, GlobalConstants.UserAgeMax)]
        public int Age { get; set; }

        [JsonProperty("Cards")]
        public ImportUserCardDto[] Cards { get; set; } = null!;
    }
}
