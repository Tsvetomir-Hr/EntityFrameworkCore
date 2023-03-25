using Artillery.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto;

[XmlType("Manufacturer")]
public class ImportManufacturerDto
{
    [XmlElement("ManufacturerName")]
    [Required]
    [MinLength(GlobalConstants.ManufarcturerNameMinLength)]
    [MaxLength(GlobalConstants.ManufarcturerNameMaxLength)]
    public string ManufacturerName { get; set; } = null!;

    [XmlElement("Founded")]
    [Required]
    [MinLength(GlobalConstants.ManufarcturerFoundedMinLength)]
    [MaxLength(GlobalConstants.ManufarcturerFoundedMaxLength)]
    public string Founded { get; set; } = null!;
}
