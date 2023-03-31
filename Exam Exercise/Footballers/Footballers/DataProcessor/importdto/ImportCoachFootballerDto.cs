using Footballers.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto;

[XmlType("Footballer")]
public class ImportCoachFootballerDto
{
    [Required]
    [MinLength(GlobalConstants.FootballerNameMinLength)]
    [MaxLength(GlobalConstants.FootballerNameMaxLength)]
    [XmlElement("Name")]
    public string Name { get; set; } = null!;

    [Required]
    [XmlElement("ContractStartDate")]
    public string ContractStartDate { get; set; } = null!;

    [Required]
    [XmlElement("ContractEndDate")]
    public string ContractEndDate { get; set; } = null!;

    [Required]
    [XmlElement("BestSkillType")]
    [Range(GlobalConstants.BestSkillMinValue, GlobalConstants.BestSkillMaxValue)]
    public string BestSkillType { get; set; } = null!;

    [Required]
    [XmlElement("PositionType")]
    [Range(GlobalConstants.PositionTypeMinValue, GlobalConstants.PositionTypeMaxValue)]
    public string PositionType { get; set; } = null!;
}
