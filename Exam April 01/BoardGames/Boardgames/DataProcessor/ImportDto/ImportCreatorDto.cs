using Boardgames.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto;

[XmlType("Creator")]
public class ImportCreatorDto
{
    [Required]
    [MinLength(GlobalConstants.CreatorNamesMinLength)]
    [MaxLength(GlobalConstants.CreatorNamesMaxLength)]
    [XmlElement("FirstName")]
    public string FirstName { get; set; } = null!;

    [Required]
    [MinLength(GlobalConstants.CreatorNamesMinLength)]
    [MaxLength(GlobalConstants.CreatorNamesMaxLength)]
    [XmlElement("LastName")]
    public string LastName { get; set; } = null!;

    [XmlArray("Boardgames")]
    public ImportCreatorBoeardGameDto[] Boardgames { get; set; }

}
