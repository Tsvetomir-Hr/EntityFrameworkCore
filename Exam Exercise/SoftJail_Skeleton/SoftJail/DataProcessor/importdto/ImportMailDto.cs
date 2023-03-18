using Newtonsoft.Json;
using SoftJail.Common;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportMailDto
    {
        [Required]
        [JsonProperty(nameof(Description))]
        public string Description { get; set; }

        [Required]
        [JsonProperty(nameof(Sender))]
        public string Sender { get; set; }

        [RegularExpression(ValidationConstants.MailAddressRegex)]
        [Required]
        [JsonProperty(nameof(Address))]
        public string Address { get; set; }
    }
}
