using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaporStore.DataProcessor.ImportDto
{
    public class ImportGameDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [Required]
        [JsonProperty("ReleaseDate")]
        public string ReleaseDate { get; set; } = null!;
        [Required]
        [JsonProperty("Developer")]
        public string DeveloperName { get; set; } = null!;
        [Required]
        [JsonProperty("Genre")]
        public string Genre { get; set; } = null!;
        [Required]
        [JsonProperty("Tags")]
        public string[] GameTags { get; set; } = null!;
    }
}
