namespace Boardgames.DataProcessor.ImportDto;


using Boardgames.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

[XmlType("Boardgame")]
public class ImportCreatorBoeardGameDto
{
    [Required]
    [MinLength(GlobalConstants.BoardGameNameMinLength)]
    [MaxLength(GlobalConstants.BoardGameNameMaxLength)]
    [XmlElement("Name")]
    public string Name { get; set; } = null!;

    [Range(GlobalConstants.BoardGameRatingMinValue,GlobalConstants.BoardGameRatingMaxValue)]
    [XmlElement("Rating")]
    public double Rating { get; set; }

    [Required]
    [Range(GlobalConstants.BoardGameYearPublishedMin,GlobalConstants.BoardGameYearPublishedMax)]
    [XmlElement("YearPublished")]
    public int YearPublished { get; set; }

    [Required]
    [Range(GlobalConstants.BoardGameCategoryTypeMinValue,GlobalConstants.BoardGameCategoryTypeMaxValue)]
    [XmlElement("CategoryType")]
    public string CategoryType { get; set; } = null!;

    [Required]
    public string Mechanics { get; set; } = null!;
}
