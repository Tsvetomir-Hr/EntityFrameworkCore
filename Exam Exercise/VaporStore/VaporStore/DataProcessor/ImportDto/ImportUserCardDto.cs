using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportUserCardDto
    {
        [Required]
        [JsonProperty("Number")]
        [RegularExpression("[0-9]{4}\\s[0-9]{4}\\s[0-9]{4}\\s[0-9]{4}")]
        public string Number { get; set; } = null!;

        [Required]
        [JsonProperty("CVC")]
        [RegularExpression(@"[0-9]{3}")]
        public string Cvc { get; set; } = null!;

        [Required]
        [JsonProperty("Type")]
        public string Type { get; set; } = null!;
    }
}
