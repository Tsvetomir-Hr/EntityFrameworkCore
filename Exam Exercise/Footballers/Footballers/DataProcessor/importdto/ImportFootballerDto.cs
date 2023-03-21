using Footballers.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto;

[XmlType("Footballer")]
public class ImportFootballerDto
{
    [XmlElement("Name")]
    [Required]
    [MinLength(ValidationConstants.FootballerNameMinLength)]
    [MaxLength(ValidationConstants.FootballerNameMaxLength)]
    public string Name { get; set; } = null!;

    [XmlElement("ContractStartDate")]
    [Required]
    public string ContractStartDate { get; set; } = null!;

    [XmlElement("ContractEndDate")]
    [Required]
    public string ContractEndDate { get; set; } = null!;

    [XmlElement("BestSkillType")]
    [Required]
    public string BestSkillType { get; set; } = null!;

    [XmlElement("PositionType")]
    [Required]
    public string PositionType { get; set; } = null!;
}
