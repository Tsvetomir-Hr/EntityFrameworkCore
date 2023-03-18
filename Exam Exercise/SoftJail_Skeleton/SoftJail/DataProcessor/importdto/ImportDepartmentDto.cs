using Newtonsoft.Json;
using SoftJail.Common;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportDepartmentDto
    {
        [JsonProperty("Name")]
        [Required]
        [MinLength(ValidationConstants.DepartmentFullNameMinLength)]
        [MaxLength(ValidationConstants.DepartmentFullNameMaxLength)]
        public string Name { get; set; }

        [JsonProperty(nameof(Cells))]
        public ImportCellDto[] Cells { get; set; }
    }
}
