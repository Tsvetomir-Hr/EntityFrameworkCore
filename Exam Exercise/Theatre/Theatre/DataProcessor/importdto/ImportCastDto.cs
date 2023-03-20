using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Theatre.Common;

namespace Theatre.DataProcessor.ImportDto;

[XmlType("Cast")]
public class ImportCastDto
{
    [XmlElement("FullName")]
    [Required]
    [MinLength(ValidationConstants.CastFullNameMinLegth)]
    [MaxLength(ValidationConstants.CastFullNameMaxLegth)]
    public string FullName { get; set; } = null!;

    [XmlElement("IsMainCharacter")]
    public bool IsMainCharacter { get; set; }

    [XmlElement("PhoneNumber")]
    [RegularExpression("(\\+44)-[0-9]{2}-[0-9]{3}-[0-9]{4}")]
    [Required]
    public string PhoneNumber { get; set; } = null!;

    [XmlElement("PlayId")]
    public int PlayId { get; set; }
}
