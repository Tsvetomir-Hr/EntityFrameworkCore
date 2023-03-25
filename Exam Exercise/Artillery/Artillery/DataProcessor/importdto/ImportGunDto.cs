using Artillery.Common;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Artillery.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportGunDto
    {
        [JsonProperty("ManufacturerId")]
        public int ManufacturerId { get; set; }

        [JsonProperty("GunWeight")]
        [Range(GlobalConstants.GunWeightMin, GlobalConstants.GunWeightMax)]
        public int GunWeight { get; set; }

        [JsonProperty("BarrelLength")]
        [Range(GlobalConstants.BarrelLengthMin, GlobalConstants.BarrelLengthMax)]
        public double BarrelLength { get; set; }

        [JsonProperty("NumberBuild")]
        public int? NumberBuild { get; set; }

        [JsonProperty("Range")]
        [Range(GlobalConstants.GunRangeMin, GlobalConstants.GunRangeMax)]
        public int Range { get; set; }

        [JsonProperty("GunType")]
        [Required]
        public string GunType { get; set; } = null!;

        [JsonProperty("ShellId")]
        public int ShellId { get; set; }

        [JsonProperty("Countries")]
        public ImportGunCountriesDto[] Countries { get; set; } = null!;
    }
}
