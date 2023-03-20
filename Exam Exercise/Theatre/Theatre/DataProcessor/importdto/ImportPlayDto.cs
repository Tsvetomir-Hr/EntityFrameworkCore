using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Theatre.Common;

namespace Theatre.DataProcessor.ImportDto;

[XmlType("Play")]
public class ImportPlayDto
{
    [XmlElement("Title")]
    [Required]
    [MinLength(ValidationConstants.PlayTtitleMinLength)]
    [MaxLength(ValidationConstants.PlayTitleMaxLength)]
    public string Title { get; set; } = null!;

    [XmlElement("Duration")]
    [Required]
    public string Duration { get; set; } = null!;

    [XmlElement("Raiting")]
    [Range(typeof(float), ValidationConstants.PlayRatingMinString, ValidationConstants.PlayRatingMaxString)]
    public float Rating { get; set; }

    [Required]
    [XmlElement("Genre")]
    public string Genre { get; set; } = null!;

    [Required]
    [MaxLength(ValidationConstants.PlayDescriptionMaxLength)]
    [XmlElement("Description")]
    public string Description { get; set; } = null!;

    [Required]
    [MinLength(ValidationConstants.PlayScreenWriterMinLength)]
    [MaxLength(ValidationConstants.PlayScreenWriterMaxLength)]
    [XmlElement("Screenwriter")]
    public string Screenwriter { get; set; } = null!;

}
