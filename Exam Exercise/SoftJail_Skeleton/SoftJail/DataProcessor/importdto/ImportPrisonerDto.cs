using System.ComponentModel.DataAnnotations;
using System;
using SoftJail.Common;
using Newtonsoft.Json;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportPrisonerDto
    {
        [Required]
        [Range(ValidationConstants.PrisonerFullNameMinLength,
            ValidationConstants.PrisonerFullNameMaxLength)]
        [JsonProperty(nameof(FullName))]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(ValidationConstants.PrisonerNicknameRegex)]
        [JsonProperty(nameof(Nickname))]
        public string Nickname { get; set; }

        [Range(ValidationConstants.PrisonerAgeMin, ValidationConstants.PrisonerAgeMax)]
        [JsonProperty(nameof(Age))]
        public int Age { get; set; }

        [Required]
        [JsonProperty(nameof(IncarcerationDate))]
        public string IncarcerationDate { get; set; }

        [JsonProperty(nameof(ReleaseDate))]
        public string ReleaseDate { get; set; }

        [Range(typeof(decimal), ValidationConstants.PrisonerBailMin, ValidationConstants.PRisonerBailMax)]
        [JsonProperty(nameof(Bail))]
        public decimal? Bail { get; set; }

        [JsonProperty(nameof(CellId))]
        public int? CellId { get; set; }

        [JsonProperty(nameof(Mails))]
        public ImportMailDto[] Mails { get; set; }

    }
}
