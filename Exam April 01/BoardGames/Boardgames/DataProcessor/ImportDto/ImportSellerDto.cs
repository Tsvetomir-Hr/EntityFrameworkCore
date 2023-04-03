using Boardgames.Common;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Boardgames.DataProcessor.ImportDto;

[JsonObject]
public class ImportSellerDto
{
    [Required]
    [MinLength(GlobalConstants.SellerNameMinLength)]
    [MaxLength(GlobalConstants.SellerNameMaxLength)]
    [JsonProperty("Name")]
    public string Name { get; set; } = null!;

    [Required]
    [MinLength(GlobalConstants.SellerAddressMinLength)]
    [MaxLength(GlobalConstants.SellerAddressMaxLength)]
    [JsonProperty("Address")]
    public string Address { get; set; } = null!;

    [Required]
    [JsonProperty("Country")]
    public string Country { get; set; }

    [Required]
    [RegularExpression("^www\\.[A-z0-9\\-]+\\.com$")]
    [JsonProperty("Website")]
    public string Website { get; set; } = null!;

    [JsonProperty("Boardgames")]
    public HashSet<int> Boardgames { get; set; }

}
